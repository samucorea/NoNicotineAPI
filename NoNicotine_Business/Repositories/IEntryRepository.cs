﻿using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public interface IEntryRepository
    {
        public Task<Entry?> GetEntryByPatientIdAsync(string patientId, CancellationToken cancellationToken);

        public Task<bool> CreateEntryAsync(Entry patient, CancellationToken cancellationToken);
    }
}
