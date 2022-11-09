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
            var patient = await _context.Patient.Include("PatientConsumptionMethods").Where(patient => patient.IdentityUserId == userId).FirstOrDefaultAsync(cancellationToken);
            if(patient == null)
            {
                return null;
            }

            return patient;
        }

        public async Task<bool> CreateEmptyPatientConsumptionMethods(string patientId, CancellationToken cancellationToken)
        {
            await _context.PatientConsumptionMethods.AddAsync(new PatientConsumptionMethods { PatientId = patientId });

            return await _context.SaveChangesAsync(cancellationToken) == 1;
        }
    }
}
