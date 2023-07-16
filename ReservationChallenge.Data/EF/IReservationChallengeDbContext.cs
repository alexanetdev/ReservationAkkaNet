using Microsoft.EntityFrameworkCore;
using ReservationChallenge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Data.EF
{
    public interface IReservationChallengeDbContext
    {
        DbSet<Client> Client { get; set; }
        DbSet<Provider> Provider { get; set; }
        DbSet<ProviderScheduleSlot> ProviderScheduleSlot { get; set; }
        Task<int> SaveChangesAsync();
    }
}
