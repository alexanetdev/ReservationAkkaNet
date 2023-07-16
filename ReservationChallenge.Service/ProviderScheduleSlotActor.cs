using Akka.Actor;
using Akka.Event;
using ReservationChallenge.Data.Repositories;
using ReservationChallenge.Dto;
using ReservationChallenge.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Service
{
    public class ProviderScheduleSlotActor : ReceiveActor
    {
        public static IActorRef Create(IActorRefFactory system, Guid providerId, IProviderScheduleRepository providerScheduleRepo)
            => system.ActorOf(Props.Create(() => new ProviderScheduleSlotActor(providerId, providerScheduleRepo)), providerId.ToString());

        private readonly Guid providerId;
        private readonly IProviderScheduleRepository providerScheduleRepo;
        private readonly ILoggingAdapter log = Logging.GetLogger(Context);

        public ProviderScheduleSlotActor(Guid providerId, IProviderScheduleRepository providerScheduleRepo)
        {
            this.providerId = providerId;
            this.providerScheduleRepo = providerScheduleRepo;

            ReceiveAsync<CreateProviderAvailability>(async m =>
            {
                if (m.StartTime - m.EndTime > TimeSpan.Zero)
                {
                    Sender.Tell(new CreateProviderAvailabilityResponse("Start time must be before End time for appointment slots."));
                    return;
                }

                if (m.StartTime < DateTimeOffset.UtcNow)
                {
                    Sender.Tell(new CreateProviderAvailabilityResponse("Start time must be after the current date time."));
                    return;
                }

                if (m.StartTime.Minute != 0 &&
                    m.StartTime.Minute != 15 &&
                    m.StartTime.Minute != 30 &&
                    m.StartTime.Minute != 45)
                {
                    var diff45 = m.StartTime.Minute - 45;
                    var diff30 = m.StartTime.Minute - 30;
                    var diff15 = m.StartTime.Minute - 15;
                    var diff0 = m.StartTime.Minute;
                    if (diff45 > 0)
                    {
                        m.StartTime = m.StartTime.AddMinutes(15 - diff45);
                    }
                    else if (diff30 > 0)
                    {
                        m.StartTime = m.StartTime.AddMinutes(15 - diff30);
                    }
                    else if (diff15 > 0)
                    {
                        m.StartTime = m.StartTime.AddMinutes(15 - diff15);
                    }
                    else
                    {
                        m.StartTime = m.StartTime.AddMinutes(15 - diff0);
                    }
                }

                var appointmentCount = (m.EndTime - m.StartTime) / m.SlotLength;

                appointmentCount = Math.Truncate(appointmentCount);

                List<ProviderScheduleSlotDto> slots = new List<ProviderScheduleSlotDto>();
                var startTime = m.StartTime;
                double start = 0;
                while (start < appointmentCount)
                {
                    var dto = await providerScheduleRepo.Create(new ProviderScheduleSlotDto(
                        Guid.NewGuid(),
                        providerId,
                        startTime,
                        startTime.Add(m.SlotLength),
                        null,
                        ProviderScheduleSlotEnum.Available
                    ));

                    slots.Add(dto);
                    startTime = startTime.Add(m.SlotLength);
                    start++;
                }

                log.Info("Available appointments created for provider {0}", providerId);

                Sender.Tell(new CreateProviderAvailabilityResponse(m.StartTime, m.StartTime.AddMinutes(15 * appointmentCount)));
            }); 
            
            ReceiveAsync<ClaimProviderTimeSlot>(async m =>
            {
                var childActor = Context.Child(m.SlotId.ToString());
                if (childActor == ActorRefs.Nobody)
                {
                    ProviderScheduleSlotChildActor.Create(Context, m.SlotId, providerScheduleRepo).Forward(m);
                }
                else
                {
                    childActor.Forward(m);
                }
            });

            ReceiveAsync<ConfirmAppointmentSlot>(async m =>
            {
                var childActor = Context.Child(m.SlotId.ToString());
                if (childActor == ActorRefs.Nobody)
                {
                    ProviderScheduleSlotChildActor.Create(Context, m.SlotId, providerScheduleRepo).Forward(m);
                }
                else
                {
                    childActor.Forward(m);
                }
            });
        }
    }
}
