using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Messages
{
    public class ClaimProviderTimeSlot
    {
        public Guid ClientId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid SlotId { get; set; }

        public ClaimProviderTimeSlot(Guid clientId, Guid providerId, Guid slotId)
        {
            ClientId = clientId;
            ProviderId = providerId;
            SlotId = slotId;
        }
    }

    public class ClaimProviderTimeSlotResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public ClaimProviderTimeSlotResponse() 
        {
            IsSuccess = true;
            ErrorMessage = string.Empty;
        }

        public ClaimProviderTimeSlotResponse(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }
    }
}
