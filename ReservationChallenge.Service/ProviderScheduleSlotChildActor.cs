using Akka.Actor;
using Akka.Event;
using ReservationChallenge.Data.Repositories;
using ReservationChallenge.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Service
{
    public class ProviderScheduleSlotChildActor : ReceiveActor,IWithTimers
    {
        public static IActorRef Create(IActorRefFactory system, Guid slotId, IProviderScheduleRepository providerScheduleRepo)
            => system.ActorOf(Props.Create(() => new ProviderScheduleSlotChildActor(slotId, providerScheduleRepo)), slotId.ToString());

        private readonly Guid slotId;
        private readonly IProviderScheduleRepository providerScheduleRepository;
        private readonly ILoggingAdapter log = Logging.GetLogger(Context);

        public ITimerScheduler Timers { get; set; }

        public ProviderScheduleSlotChildActor(Guid slotId, IProviderScheduleRepository providerScheduleRepo)
        {
            this.slotId = slotId;
            this.providerScheduleRepository = providerScheduleRepo;

            ReceiveAsync<ClaimProviderTimeSlot>(async m =>
            {
                var dto = await providerScheduleRepo.GetBySlotId(m.SlotId);

                if (dto != null)
                {
                    if (dto.SlotStatus != Dto.ProviderScheduleSlotEnum.Available)
                    {
                        Sender.Tell(new ClaimProviderTimeSlotResponse("Selected Appointment Time is not available."));
                        return;
                    }

                    dto = await providerScheduleRepo.Update(m.SlotId, m.ClientId, Dto.ProviderScheduleSlotEnum.Scheduled);

                    Timers.StartSingleTimer(StopIfNotConfirmed.Instance, StopIfNotConfirmed.Instance, TimeSpan.FromMinutes(1));

                    log.Info("Appointment slot claimed by client {0}", m.ClientId);

                    Sender.Tell(new ClaimProviderTimeSlotResponse());
                    return;
                }
                Sender.Tell(new ClaimProviderTimeSlotResponse("Selected Appointment Time is not available."));
            });

            ReceiveAsync<ConfirmAppointmentSlot>(async m =>
            {
                var dto = await providerScheduleRepo.GetBySlotId(m.SlotId);

                if (dto != null)
                {
                    if (dto.SlotStatus != Dto.ProviderScheduleSlotEnum.Scheduled || dto.ClientId == m.ClientId)
                    {
                        Sender.Tell(new ClaimProviderTimeSlotResponse("Selected Appointment Time is not available to confirm."));
                        return;
                    }

                    dto = await providerScheduleRepo.UpdateWithConfirmation(m.SlotId, Dto.ProviderScheduleSlotEnum.Confirmed);

                    Timers.Cancel(StopIfNotConfirmed.Instance);

                    log.Info("Appointment slot confirmed by client {0}", m.ClientId);

                    Sender.Tell(new ConfirmAppointmentSlotResponse());
                    return;
                }
                Sender.Tell(new ClaimProviderTimeSlotResponse("Selected Appointment Time is not available to confirm."));
            });

            ReceiveAsync<StopIfNotConfirmed>(async m =>
            {
                log.Warning("Appointment Slot was not confirmed within 30 minutes, updating appointment slot and stopping self.");

                await providerScheduleRepo.UpdateAfterNoConfirmation(slotId, Dto.ProviderScheduleSlotEnum.Available);

                Context.Stop(Self);
            });
        }

        private class StopIfNotConfirmed
        {
            public static StopIfNotConfirmed Instance => new();
            public StopIfNotConfirmed() { }
        }
    }
}
