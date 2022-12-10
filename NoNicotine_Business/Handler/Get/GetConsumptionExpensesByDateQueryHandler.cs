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
    public class GetConsumptionExpensesByDateQueryHandler : IRequestHandler<GetConsumptionExpensesByDateQuery, Response<ConsumptionExpensesResponse>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetConsumptionExpensesByDateQueryHandler> _logger;
        private readonly IPatientConsumptionMethodsRepository _patientConsumptionMethodsRepository;
        public GetConsumptionExpensesByDateQueryHandler(AppDbContext context, ILogger<GetConsumptionExpensesByDateQueryHandler> logger, IPatientConsumptionMethodsRepository consumptionMethodsRepository)
        {
            _context = context;
            _logger = logger;
            _patientConsumptionMethodsRepository = consumptionMethodsRepository;
        }
        public async Task<Response<ConsumptionExpensesResponse>> Handle(GetConsumptionExpensesByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _context.Patient.Where(x => x.ID == request.PatientId).FirstOrDefaultAsync(cancellationToken);

                if (patient == null)
                {
                    return new Response<ConsumptionExpensesResponse>()
                    {
                        Message = "Invalid Patient Id",
                        Succeeded = false
                    };
                }
                
                if (patient.StartTime > request.Since ||  patient.StartTime > request.Until ||  DateTime.Today < request.Until  || DateTime.Today < request.Since)
                {
                    return new Response<ConsumptionExpensesResponse>()
                    {
                        Message = "Invalid date range",
                        Succeeded = false
                    };
                }

                var dailyConsumption = await _patientConsumptionMethodsRepository.CalculateDailyConsumption(patient.PatientConsumptionMethodsId,cancellationToken);

                int multiplier = (int)(request.Since - request.Until).TotalDays;

                if (dailyConsumption == null)
                {
                    return new Response<ConsumptionExpensesResponse>()
                    {
                        Succeeded = false,
                        Message ="Patient do not have consumption methods"
                    };
                }

                return new Response<ConsumptionExpensesResponse>()
                {
                    Succeeded = true,
                    Data = new ConsumptionExpensesResponse()
                    {
                        Total = dailyConsumption.Total * multiplier,
                        Cigarette = dailyConsumption.Cigarette * multiplier,
                        ElectronicCigarette = dailyConsumption.ElectronicCigarette * multiplier,
                        Cigar = dailyConsumption.Cigar * multiplier,
                        Hookah = dailyConsumption.Hookah * multiplier,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting patient consumption expenses: {errMessage}", ex.Message);
                return new Response<ConsumptionExpensesResponse>()
                {
                    Message = "Somenthing went wrong",
                    Succeeded = false
                };
                
            }
        }
    }
}
