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
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientConsumptionMethodsRepository _patientConsumptionMethodsRepository;

        public GetConsumptionExpensesQueryHandler(IPatientRepository patientRepository, IPatientConsumptionMethodsRepository consumptionMethodsRepository)
        {
            _patientRepository = patientRepository;
            _patientConsumptionMethodsRepository = consumptionMethodsRepository;
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

            var dailyConsumption = await _patientConsumptionMethodsRepository.CalculateDailyConsumption(patient.PatientConsumptionMethodsId, cancellationToken);

            if (dailyConsumption == null)
            {
                return new Response<ConsumptionExpensesResponse>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            var (cigaretteDaily, cigarDaily, electronicCigaretteDaily, hookahDaily) = (dailyConsumption.Cigarette, dailyConsumption.Cigar, dailyConsumption.ElectronicCigarette, dailyConsumption.Hookah);
            int totalDaily = cigaretteDaily + cigarDaily + electronicCigaretteDaily + hookahDaily;
            int multiplier = (int)(DateTime.Now - patient.StartTime).TotalDays;

            return new Response<ConsumptionExpensesResponse>()
            {
                Succeeded = true,
                Data = new ConsumptionExpensesResponse()
                {
                    Total = totalDaily * multiplier,
                    Cigarette = cigaretteDaily * multiplier,
                    ElectronicCigarette = electronicCigaretteDaily * multiplier,
                    Cigar = cigarDaily * multiplier,
                    Hookah = hookahDaily * multiplier,
                }
            };
        }
    }
}
