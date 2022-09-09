using Microsoft.EntityFrameworkCore;
using NoNicotineAPI.Models;

namespace NoNicotineAPI
{
    public class NoNicotineContext : DbContext
    {
        public NoNicotineContext(DbContextOptions<NoNicotineContext> options) : base(options)
        {

        }

        public DbSet<Entry> Entry { get; set; }
        public DbSet<Feeling> Feeling { get; set; }
        public DbSet<Habit> Habit { get; set; }
        public DbSet<IdentificationType> IdentificationType { get; set; }
        public DbSet<LinkRequest> LinkRequest { get; set; }
        public DbSet<LinkRequestStatus> LinkRequestStatus { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientHabit> PatientHabit { get; set; }
        public DbSet<PatientRelapseHistory> PatientRelapseHistory { get; set; }
        public DbSet<Symptom> Symptom { get; set; }
        public DbSet<Therapist> Therapist { get; set; }

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
