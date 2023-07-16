using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Messages
{
    public class CreateProviderAvailability
    {
        public Guid ProviderId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public TimeSpan SlotLength { get; set; } 

        public CreateProviderAvailability(Guid providerId, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            ProviderId = providerId;
            StartTime = startTime;
            EndTime = endTime;
            SlotLength = TimeSpan.FromMinutes(15);
        }
    }

    public class CreateProviderAvailabilityResponse
    {
        public bool IsSuccess { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string ErrorMessage { get; set; }

        public CreateProviderAvailabilityResponse(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            IsSuccess = true;
            StartTime = startTime;
            EndTime = endTime;
            ErrorMessage = string.Empty;
        }

        public CreateProviderAvailabilityResponse(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            StartTime = DateTimeOffset.MinValue;
            EndTime = DateTimeOffset.MinValue;
        }
    }
}
