using Akka.Actor;
using Akka.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using ReservationChallenge.Api.Models;
using ReservationChallenge.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProviderScheduleSlotController : Controller
    {
        protected IActorRegistry _registry;

        public ProviderScheduleSlotController(IActorRegistry registry)
        {
            this._registry = registry;
        }

        [Route("{providerId}/createAvailability")]
        [HttpPost]
        public async Task<IActionResult> CreateProviderAvailability([FromBody]CreateProviderAvailability obj, Guid providerId)
        {
            var request = new Messages.CreateProviderAvailability(providerId, obj.StartTime, obj.EndTime);

            var actor = _registry.Get<ReservationChallengeActor>();

            var response = actor.Ask(request, TimeSpan.FromSeconds(5));

            return Ok(response);
        }

        [Route("{providerId}/slot/{slotId}/client/{clientId}/schedule")]
        [HttpPost]
        public async Task<IActionResult> ClaimProviderTimeSlot(Guid providerId, Guid clientId, Guid slotId)
        {
            var request = new Messages.ClaimProviderTimeSlot(clientId, providerId, slotId);

            var actor = _registry.Get<ReservationChallengeActor>();

            var response = actor.Ask(request, TimeSpan.FromSeconds(5));

            return Ok(response);
        }

        [Route("{providerId}/slot/{slotId}/client/{clientId}/confirm")]
        [HttpPost]
        public async Task<IActionResult> ConfirmProviderTimeSlot(Guid providerId, Guid clientId, Guid slotId)
        {
            var request = new Messages.ConfirmAppointmentSlot(clientId, providerId, slotId);

            var actor = _registry.Get<ReservationChallengeActor>();

            var response = actor.Ask(request, TimeSpan.FromSeconds(5));

            return Ok(response);
        }
    }
}
