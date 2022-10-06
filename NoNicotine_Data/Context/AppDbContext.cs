using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Entry> Entry { get; set; }
        public DbSet<Feeling> Feeling { get; set; }
        public DbSet<Habit> Habit { get; set; }
        public DbSet<HabitSchedule> HabitSchedule { get; set; }
        public DbSet<LinkRequest> LinkRequest { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientHabit> PatientHabit { get; set; }
        public DbSet<PatientRelapseHistory> PatientRelapseHistory { get; set; }
        public DbSet<Symptom> Symptom { get; set; }
        public DbSet<Therapist> Therapist { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var decimalProps = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
