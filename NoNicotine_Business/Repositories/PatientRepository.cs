using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        private readonly AppDbContext _context;
        public PatientsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreatePatientAsync(Patient patient)
        {
            await _context.Patient.AddAsync(patient);

            return await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(string id)
        {
            var patient = await _context.Patient.Where(patient => patient.ID == id).ToListAsync();
            if (patient == null)
            {
                return null;
            }

            return patient.FirstOrDefault();
        }
    }
}
