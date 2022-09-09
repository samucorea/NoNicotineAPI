using Microsoft.EntityFrameworkCore;
using NoNicotineAPI.Models;

namespace NoNicotineAPI
{
    public class NoNicotineContext : DbContext
    {
        public NoNicotineContext(DbContextOptions<NoNicotineContext> options) : base(options)
        {

        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Feeling> Feelings { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<IdentificationType> IdentificationTypes { get; set; }
        public DbSet<LinkRequest> LinkRequests { get; set; }
        public DbSet<LinkRequestStatus> LinkRequestStatuses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRelapseHistory> PatientRelapseHistoric { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Therapist> Therapists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
