using ReservationChallenge.Data.EF;
using ReservationChallenge.Data.Models;
using ReservationChallenge.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Data.Repositories
{
    public class ProviderScheduleRepository : IProviderScheduleRepository
    {
        private readonly IReservationChallengeDbContext context;

        public ProviderScheduleRepository(IReservationChallengeDbContext context)
        {
            this.context = context;
        }

        public async Task<ProviderScheduleSlotDto> Create(ProviderScheduleSlotDto obj)
        {
            var entity = EFExtensions.To(obj);

            context.ProviderScheduleSlot.Add(entity);
            await context.SaveChangesAsync();

            return EFExtensions.From(entity);
        }

        public async Task<ProviderScheduleSlotDto> GetBySlotId(Guid slotId)
        {
            var entity = context.ProviderScheduleSlot.FirstOrDefault(pss => pss.Id == slotId);

            return EFExtensions.From(entity);
        }

        public async Task<ProviderScheduleSlotDto> Update(Guid slotId, Guid clientId, ProviderScheduleSlotEnum status)
        {
            var entity = context.ProviderScheduleSlot.FirstOrDefault(pss => pss.Id == slotId);

            entity.ClientId = clientId.ToString();
            entity.SlotStatus = ((int)status);
            context.ProviderScheduleSlot.Update(entity);
            await context.SaveChangesAsync();

            return EFExtensions.From(entity);
        }

        public async Task<ProviderScheduleSlotDto> UpdateAfterNoConfirmation(Guid slotId, ProviderScheduleSlotEnum status)
        {
            var entity = context.ProviderScheduleSlot.FirstOrDefault(pss => pss.Id == slotId);

            entity.ClientId = null;
            entity.SlotStatus = ((int)status);
            context.ProviderScheduleSlot.Update(entity);
            await context.SaveChangesAsync();

            return EFExtensions.From(entity);
        }

        public async Task<ProviderScheduleSlotDto> UpdateWithConfirmation(Guid slotId, ProviderScheduleSlotEnum status)
        {
            var entity = context.ProviderScheduleSlot.FirstOrDefault(pss => pss.Id == slotId);

            entity.SlotStatus = ((int)status);
            context.ProviderScheduleSlot.Update(entity);
            await context.SaveChangesAsync();

            return EFExtensions.From(entity);
        }
    }
}
