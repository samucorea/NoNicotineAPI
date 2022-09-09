﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal DailyConsumption { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime AffiliationDate { get; set; }

        public Guid TherapistId { get; set; }
        public Therapist Therapist { get; set; }
        public List<Entry> Entries { get; set; }
        public ICollection<LinkRequest> LinkRequests { get; set; }
        public List<PatientRelapseHistory> PatientRelapseHistoric { get; set; }
        public ICollection<Habit> Habits { get; set; }
    }
}
