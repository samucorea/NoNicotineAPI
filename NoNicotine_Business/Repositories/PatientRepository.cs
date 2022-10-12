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

        public async Task<Patient?> GetPatientByIdAsync(string id)
        {
            var patient = await _context.Patient.FindAsync(id);
            if(patient == null)
            {
                return null;
            }

            return patient;
        }
    }
}
