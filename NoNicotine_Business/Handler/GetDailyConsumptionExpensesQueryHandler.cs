using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler
{
    public class GetDailyConsumptionExpensesQueryHandler : IRequestHandler<GetDailyConsumptionExpensesQuery, Response<DailyConsumptionResponse>>
    {
        private readonly AppDbContext _context;
        public GetDailyConsumptionExpensesQueryHandler(AppDbContext context)
        {
            _context = context;
        }
    

        public async Task<Response<DailyConsumptionResponse>> Handle(GetDailyConsumptionExpensesQuery request, CancellationToken cancellationToken)
        {

            var patientConsumptionMethods = await _context.PatientConsumptionMethods
                .Include("ElectronicCigaretteDetails")
                .Include("CigarDetails")
                .Include("CigaretteDetails")
                .Include("HookahDetails")
                .Where(p => p.ID == request.PatientConsumptionMethodsId).FirstOrDefaultAsync(cancellationToken);
            if(patientConsumptionMethods == null)
            {
                return new Response<DailyConsumptionResponse>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            int cigarExpenses = CalculateCigarConsumptionExpense(patientConsumptionMethods.CigarDetails);
            int cigaretteExpenses = CalculateCigaretteConsumptionExpense(patientConsumptionMethods.CigaretteDetails);
            int electronicCigaretteExpenses = CalculateElectronicCigaretteConsumptionExpense(patientConsumptionMethods.ElectronicCigaretteDetails);
            int hookahExpenses = CalculateHookahConsumptionExpense(patientConsumptionMethods.HookahDetails);

            int total = cigaretteExpenses + cigarExpenses + electronicCigaretteExpenses + hookahExpenses;

            return new Response<DailyConsumptionResponse>()
            {
                Succeeded = true,
                Data = new DailyConsumptionResponse()
                {
                    Value = total
                }
            };
        }

        private static int CalculateCigarConsumptionExpense(CigarDetails? cigarDetails)
        {
            if(cigarDetails == null)
            {
                return 0;
            }

            return (int)(cigarDetails.boxPrice / cigarDetails.unitsPerBox * cigarDetails.unitsPerDay * 7 / cigarDetails.daysPerWeek / 7);
        }

        private static int CalculateCigaretteConsumptionExpense(CigaretteDetails? cigaretteDetails)
        {
            if(cigaretteDetails == null)
            {
                return 0;
            }

            return (int)(cigaretteDetails.boxPrice / cigaretteDetails.unitsPerBox * cigaretteDetails.unitsPerDay * 7 / cigaretteDetails.daysPerWeek / 7);
        }

        private static int CalculateElectronicCigaretteConsumptionExpense(ElectronicCigaretteDetails? electronicCigaretteDetails)
        {
            if(electronicCigaretteDetails == null)
            {
                return 0;
            }
            
            return (int)(electronicCigaretteDetails.boxPrice / electronicCigaretteDetails.unitsPerBox / electronicCigaretteDetails.cartridgeLifespan / 7);
        }

        private static int CalculateHookahConsumptionExpense(HookahDetails? hookahDetails)
        {
            if(hookahDetails == null)
            {
                return 0;
            }

            return (int)(hookahDetails.setupPrice * hookahDetails.daysPerWeek / 7);
        }
    }
}
