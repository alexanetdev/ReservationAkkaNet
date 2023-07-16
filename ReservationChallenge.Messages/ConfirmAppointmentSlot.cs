using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Messages
{
    public class ConfirmAppointmentSlot
    {
        public Guid ClientId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid SlotId { get; set; }

        public ConfirmAppointmentSlot(Guid clientId, Guid providerId, Guid slotId)
        {
            ClientId = clientId;
            ProviderId = providerId;
            SlotId = slotId;
        }
    }

    public class ConfirmAppointmentSlotResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public ConfirmAppointmentSlotResponse()
        {
            IsSuccess = true;
            ErrorMessage = string.Empty;
        }

        public ConfirmAppointmentSlotResponse(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;

        }
    }
}
