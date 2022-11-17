using Microsoft.EntityFrameworkCore;
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

            var patientConsumptionMethods = await _context.PatientConsumptionMethods.Where(pcm => pcm.PatientId == patient.ID).FirstOrDefaultAsync();
            if(patientConsumptionMethods == null)
            {
                return null;
            }

            patient.PatientConsumptionMethodsId = patientConsumptionMethods.ID;

            return patient;
        }

        public async Task<PatientConsumptionMethods?> CreateEmptyPatientConsumptionMethods(string patientId, CancellationToken cancellationToken)
        {
            var patientConsumptionMethods = new PatientConsumptionMethods { PatientId = patientId };
            await _context.PatientConsumptionMethods.AddAsync(patientConsumptionMethods);

            var result =  await _context.SaveChangesAsync(cancellationToken);
            if (result < 1)
            {
                return null;
            }

            return patientConsumptionMethods;
        }

        public async Task<List<Patient>> GetTherapistPatientsAsync(string therapistId, CancellationToken cancellationToken)
        {
            var patients = await _context.Patient.Where(patient => patient.TherapistId == therapistId).ToListAsync(cancellationToken);

            return patients;
        }
    }
}
