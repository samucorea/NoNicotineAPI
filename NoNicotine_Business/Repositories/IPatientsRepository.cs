using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public interface IPatientsRepository
    {
        public Task<Patient> GetPatientByIdAsync(string id);

        public Task<int> CreatePatientAsync(Patient patient);
    }
}
