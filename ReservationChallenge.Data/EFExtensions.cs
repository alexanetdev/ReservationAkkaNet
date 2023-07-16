using ReservationChallenge.Data.Models;
using ReservationChallenge.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Data
{
    public static class EFExtensions
    {
        public static ProviderScheduleSlotDto From(ProviderScheduleSlot obj)
        {
            return new ProviderScheduleSlotDto(
                obj.Id,
                Guid.Parse(obj.ProviderId),
                obj.StartTime,
                obj.EndTime,
                obj.ClientId == null ? null : Guid.Parse(obj.ClientId),
                (ProviderScheduleSlotEnum)obj.SlotStatus
                );
        }

        public static ProviderScheduleSlot To(ProviderScheduleSlotDto obj)
        {
            return new ProviderScheduleSlot {
                Id = obj.Id,
                ProviderId = obj.ProviderId.ToString(),
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                ClientId = obj.ClientId == null ? null : obj.ClientId.ToString(),
                SlotStatus = ((int)obj.SlotStatus)
                };
        }

        public static ProviderDto From(Provider obj)
        {
            return new ProviderDto(Guid.Parse(obj.Id), obj.Name);
        }

        public static ClientDto From(Client obj)
        {
            return new ClientDto(Guid.Parse(obj.Id), obj.Name);
        }
    }
}
