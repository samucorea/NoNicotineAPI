using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Get
{
    public class GetConsumptionExpensesQueryHandler : IRequestHandler<GetConsumptionExpensesQuery, Response<ConsumptionExpensesResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IPatientRepository _patientRepository;
        public GetConsumptionExpensesQueryHandler(AppDbContext context, IPatientRepository patientRepository)
        {
            _context = context;
            _patientRepository = patientRepository;
        }

        public async Task<Response<ConsumptionExpensesResponse>> Handle(GetConsumptionExpensesQuery request, CancellationToken cancellationToken)
        {

            var patient = await _patientRepository.GetPatientByUserIdAsync(request.UserId, cancellationToken);

            if (patient == null)
            {
                return new Response<ConsumptionExpensesResponse>()
                {
                    Succeeded = false,
                    Message = "Patient not found with specified id"
                };
            }

            var patientConsumptionMethods = await _context.PatientConsumptionMethods
                .Include("ElectronicCigaretteDetails")
                .Include("CigarDetails")
                .Include("CigaretteDetails")
                .Include("HookahDetails")
                .Where(p => p.ID == patient.PatientConsumptionMethodsId).FirstOrDefaultAsync(cancellationToken);
            if (patientConsumptionMethods == null)
            {
                return new Response<ConsumptionExpensesResponse>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            int cigarExpenses = CalculateCigarConsumptionExpense(patientConsumptionMethods.CigarDetails);
            int cigaretteExpenses = CalculateCigaretteConsumptionExpense(patientConsumptionMethods.CigaretteDetails);
            int electronicCigaretteExpenses = CalculateElectronicCigaretteConsumptionExpense(patientConsumptionMethods.ElectronicCigaretteDetails);
            int hookahExpenses = CalculateHookahConsumptionExpense(patientConsumptionMethods.HookahDetails);           

            int dailySavings = cigaretteExpenses + cigarExpenses + electronicCigaretteExpenses + hookahExpenses;
            int multiplier = (int)(DateTime.Now - patient.StartTime).TotalDays;

            return new Response<ConsumptionExpensesResponse>()
            {
                Succeeded = true,
                Data = new ConsumptionExpensesResponse()
                {
                    Total = dailySavings * multiplier,
                    CigaretteTotal = cigaretteExpenses * multiplier,
                    ElectronicCigaretteTotal = electronicCigaretteExpenses * multiplier,
                    CigarTotal = cigarExpenses * multiplier,
                    HookahTotal = hookahExpenses * multiplier,
                }
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
