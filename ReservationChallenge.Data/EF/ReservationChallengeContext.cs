using Microsoft.EntityFrameworkCore;
using ReservationChallenge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationChallenge.Data.EF
{
    public class ReservationChallengeContext : DbContext,IReservationChallengeDbContext
    {
        public ReservationChallengeContext() { }

        public ReservationChallengeContext(DbContextOptions<ReservationChallengeContext> options) 
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        public virtual DbSet<ProviderScheduleSlot> ProviderScheduleSlot { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<ProviderScheduleSlot>(entity =>
            {
                entity.HasKey(pss => pss.Id);

                entity.HasOne(pss => pss.Provider)
                    .WithMany(p => p.ProviderScheduleSlots)
                    .HasForeignKey(pss => pss.ProviderId);

                entity.HasOne(pss => pss.Client)
                    .WithMany(c => c.ProviderScheduleSlots)
                    .HasForeignKey(pss => pss.ClientId);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
