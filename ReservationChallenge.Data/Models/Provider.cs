using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Data.Models
{
    public class Provider
    {
        public string Id { get; set; }

        //Added to fill out perceived record
        public string Name { get; set; }
        public virtual ICollection<ProviderScheduleSlot> ProviderScheduleSlots { get; set; }

        //Additional properties continue if required

    }
}
