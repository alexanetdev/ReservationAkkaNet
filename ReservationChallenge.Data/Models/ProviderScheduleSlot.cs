using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Data.Models
{
    public class ProviderScheduleSlot
    {
        public Guid Id { get; set; }
        public string ProviderId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int SlotStatus { get; set; }
        public string? ClientId { get; set; }

        public virtual Provider Provider { get; set; }
        public virtual Client? Client { get; set; }

        //Additional properties continue if required
    }
}
