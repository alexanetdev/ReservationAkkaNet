using ReservationChallenge.Data.Models;
using ReservationChallenge.Dto;

namespace ReservationChallenge.Data.Repositories
{
    public interface IProviderScheduleRepository
    {
        Task<ProviderScheduleSlotDto> GetBySlotId(Guid slotId);
        Task<ProviderScheduleSlotDto> Create(ProviderScheduleSlotDto obj);

        Task<ProviderScheduleSlotDto> Update(Guid slotId, Guid clientId, ProviderScheduleSlotEnum status);
        Task<ProviderScheduleSlotDto> UpdateAfterNoConfirmation(Guid slotId, ProviderScheduleSlotEnum status);
        Task<ProviderScheduleSlotDto> UpdateWithConfirmation(Guid slotId, ProviderScheduleSlotEnum status);
    }
}
