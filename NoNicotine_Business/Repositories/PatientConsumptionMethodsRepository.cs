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
    public class PatientConsumptionMethodsRepository : IPatientConsumptionMethodsRepository
    {
        private readonly AppDbContext _context;
        public PatientConsumptionMethodsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ConsumptionExpensesResponse?> CalculateDailyConsumption(string patientConsumptionMethodsId, CancellationToken cancellationToken)
        {
            var patientConsumptionMethods = await _context.PatientConsumptionMethods
                .Include("ElectronicCigaretteDetails")
                .Include("CigarDetails")
                .Include("CigaretteDetails")
                .Include("HookahDetails")
                .Where(p => p.ID == patientConsumptionMethodsId).FirstOrDefaultAsync(cancellationToken);

            if (patientConsumptionMethods == null)
            {
                return null;
            }

            int cigarExpenses = CalculateCigarConsumptionExpense(patientConsumptionMethods.CigarDetails);
            int cigaretteExpenses = CalculateCigaretteConsumptionExpense(patientConsumptionMethods.CigaretteDetails);
            int electronicCigaretteExpenses = CalculateElectronicCigaretteConsumptionExpense(patientConsumptionMethods.ElectronicCigaretteDetails);
            int hookahExpenses = CalculateHookahConsumptionExpense(patientConsumptionMethods.HookahDetails);

            return new ConsumptionExpensesResponse
            {
                Cigarette = cigaretteExpenses,
                ElectronicCigarette = electronicCigaretteExpenses,
                Cigar = cigarExpenses,
                Hookah = hookahExpenses,
            };

        }
        private static int CalculateCigarConsumptionExpense(CigarDetails? cigarDetails)
        {
            if (cigarDetails == null)
            {
                return 0;
            }

            return (int)(cigarDetails.boxPrice / cigarDetails.unitsPerBox * cigarDetails.unitsPerDay * 7 / cigarDetails.daysPerWeek / 7);
        }

        private static int CalculateCigaretteConsumptionExpense(CigaretteDetails? cigaretteDetails)
        {
            if (cigaretteDetails == null)
            {
                return 0;
            }

            return (int)(cigaretteDetails.boxPrice / cigaretteDetails.unitsPerBox * cigaretteDetails.unitsPerDay * 7 / cigaretteDetails.daysPerWeek / 7);
        }

        private static int CalculateElectronicCigaretteConsumptionExpense(ElectronicCigaretteDetails? electronicCigaretteDetails)
        {
            if (electronicCigaretteDetails == null)
            {
                return 0;
            }

            return (int)(electronicCigaretteDetails.boxPrice / electronicCigaretteDetails.unitsPerBox / electronicCigaretteDetails.cartridgeLifespan / 7);
        }

        private static int CalculateHookahConsumptionExpense(HookahDetails? hookahDetails)
        {
            if (hookahDetails == null)
            {
                return 0;
            }

            return (int)(hookahDetails.setupPrice * hookahDetails.daysPerWeek / 7);
        }
    }
}
