namespace ReservationChallenge.Dto
{
    public class ProviderScheduleSlotDto
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public Guid? ClientId { get; set; }
        public ProviderScheduleSlotEnum SlotStatus { get; set; }

        public ProviderScheduleSlotDto(
            Guid id,
            Guid providerId,
            DateTimeOffset startTime,
            DateTimeOffset endTime, 
            Guid? clientId, 
            ProviderScheduleSlotEnum slotStatus)
        {
            Id = id;
            ProviderId = providerId;
            StartTime = startTime;
            EndTime = endTime;
            ClientId = clientId;
            SlotStatus = slotStatus;
        }

        //Additional properties continue if required
    }
}
