using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Context
{
    public class AppDbContextAdapter : IAppDbContext
    {
        private readonly AppDbContext _context;

        public AppDbContextAdapter(AppDbContext context)
        {
            _context = context;
        }


        public DbSet<Entry> Entry { get { return _context.Entry; } }
        public DbSet<Feeling> Feeling { get { return _context.Feeling; } }
        public DbSet<Habit> Habit { get { return _context.Habit; } }
        public DbSet<HabitSchedule> HabitSchedule { get { return _context.HabitSchedule; } }
        public DbSet<LinkRequest> LinkRequest { get { return _context.LinkRequest; } }
        public virtual DbSet<Patient> Patient { get { return _context.Patient; } }
        public DbSet<PatientHabit> PatientHabit { get { return _context.PatientHabit; } }
        public DbSet<PatientRelapseHistory> PatientRelapseHistory { get { return _context.PatientRelapseHistory; } }
        public DbSet<Symptom> Symptom { get { return _context.Symptom; } }
        public DbSet<Therapist> Therapist { get { return _context.Therapist; } }

        public DbSet<RefreshToken> RefreshToken { get { return _context.RefreshToken; } }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
