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
    public interface IAppDbContext
    {
        IDbContextTransaction BeginTransaction();

        public DbSet<Entry> Entry { get;}
        public DbSet<Feeling> Feeling { get;  }
        public DbSet<Habit> Habit { get;  }
        public DbSet<HabitSchedule> HabitSchedule { get; }
        public DbSet<LinkRequest> LinkRequest { get;  }
        public DbSet<Patient> Patient { get;  }
        public DbSet<PatientHabit> PatientHabit { get;  }
        public DbSet<PatientRelapseHistory> PatientRelapseHistory { get;  }
        public DbSet<Symptom> Symptom { get; }
        public DbSet<Therapist> Therapist { get;  }

        public DbSet<RefreshToken> RefreshToken { get;  }
    }
}
