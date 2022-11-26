using Microsoft.EntityFrameworkCore;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreatePatientAsync(Patient patient, CancellationToken cancellationToken)
        {
            await _context.Patient.AddAsync(patient, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Patient?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == userId).FirstOrDefaultAsync(cancellationToken);
            if(patient == null)
            {
                return null;
            }

            return patient;
        }

        public async Task<TherapistPatient?> GetTherapistPatientAsync(string therapistId, string patientId, CancellationToken cancellationToken)
        {
            var patient = await _context.Patient.Where(patient => patient.ID == patientId && patient.TherapistId == therapistId).FirstOrDefaultAsync(cancellationToken);

            if (patient == null)
            {
                return null;
            }

            TherapistPatient therapistPatient = new()
            {
                Name = patient.Name,
                Sex = patient.Sex,
                BirthDate = patient.BirthDate,
                StartTime = patient.StartTime,
                Active = patient.Active,
                TherapistId = patient.TherapistId,
                PatientConsumptionMethodsId = patient.PatientConsumptionMethodsId,
                PatientConsumptionMethods = patient.PatientConsumptionMethods,
                ID = patient.ID,
                CreatedAt = patient.CreatedAt
            };

            return therapistPatient;
        }

        public async Task<List<TherapistPatient>?> GetTherapistPatientsAsync(string therapistId, CancellationToken cancellationToken)
        {
            var patients = await _context.Patient.Where(patient => patient.TherapistId == therapistId).ToListAsync(cancellationToken);                    
            
            if (patients.Count < 1)
            {
                return null;
            }

            List<TherapistPatient> therapistPatients = new List<TherapistPatient>();

            foreach (var patient in patients)
            {
                TherapistPatient therapistPatient = new()
                {
                    Name = patient.Name,
                    Sex = patient.Sex,
                    BirthDate = patient.BirthDate,
                    StartTime = patient.StartTime,
                    Active = patient.Active,
                    TherapistId = patient.TherapistId,
                    PatientConsumptionMethodsId = patient.PatientConsumptionMethodsId,
                    PatientConsumptionMethods = patient.PatientConsumptionMethods,
                    ID = patient.ID,
                    CreatedAt = patient.CreatedAt
                };
                therapistPatients.Add(therapistPatient);
            }

            return therapistPatients;
        }
    }
}
