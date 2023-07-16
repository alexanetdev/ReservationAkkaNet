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
    public class ReservationChallengeActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Logging.GetLogger(Context);

        private readonly IProviderScheduleRepository providerScheduleRepo;

        public ReservationChallengeActor(IProviderRepository providerRepo, IProviderScheduleRepository providerScheduleRepo)
        {
            this.providerScheduleRepo = providerScheduleRepo;

            log.Info("Started ReservationChallengeActor");

            ReceiveAsync<CreateProviderAvailability>(async m =>
            {
                var childActor = Context.Child(m.ProviderId.ToString());
                if (childActor == ActorRefs.Nobody)
                {
                    ProviderScheduleSlotActor.Create(Context, m.ProviderId, providerScheduleRepo).Forward(m);
                }
                else
                {
                    childActor.Forward(m);
                }
            });

            ReceiveAsync<ClaimProviderTimeSlot>(async m =>
            {
                var childActor = Context.Child(m.ProviderId.ToString());
                if (childActor == ActorRefs.Nobody)
                {
                    ProviderScheduleSlotActor.Create(Context, m.ProviderId, providerScheduleRepo).Forward(m);
                }
                else
                {
                    childActor.Forward(m);
                }
            });

            ReceiveAsync<ConfirmAppointmentSlot>(async m =>
            {
                var childActor = Context.Child(m.ProviderId.ToString());
                if (childActor == ActorRefs.Nobody)
                {
                    ProviderScheduleSlotActor.Create(Context, m.ProviderId, providerScheduleRepo).Forward(m);
                }
                else
                {
                    childActor.Forward(m);
                }
            });
        }
    }
}
