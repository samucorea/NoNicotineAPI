using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public interface IPatientRepository
    {
        public Task<PatientDTO?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken);

        public Task<int> CreatePatientAsync(Patient patient, CancellationToken cancellationToken);

        public Task<TherapistPatient?> GetTherapistPatientAsync(string therapistId, string patientId, CancellationToken cancellationToken);

        public Task<List<TherapistPatient>?> GetTherapistPatientsAsync(string therapistId, CancellationToken cancellationToken);
    }
}
