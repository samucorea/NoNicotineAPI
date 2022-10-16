using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
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
    public class CreateHookaDetailsCommandHandler : IRequestHandler<CreateHookaDetailsCommand, Response<HookahDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateHookaDetailsCommandHandler> _logger;
        public CreateHookaDetailsCommandHandler(AppDbContext context, ILogger<CreateHookaDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<HookahDetails>> Handle(CreateHookaDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // validation 
                var isValidation = await ValidationRequest(request);
                if (isValidation is not null)
                {
                    return isValidation;
                }
                // patient consumption method
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);

                var isHookaDetails = new HookahDetails()
                {
                    setupPrice = request.setupPrice,
                    daysPerWeek = request.daysPerWeek,
                    PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
                };
               
                await _context.HookahDetails.AddAsync(isHookaDetails);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<HookahDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                // updates relationship with patient comsumption method
                isPatientConsumption.HookahDetailsId = isHookaDetails.ID;
                _context.PatientConsumptionMethods.Update(isPatientConsumption);
                await _context.SaveChangesAsync();

                return new Response<HookahDetails>()
                {
                    Succeeded = true,
                    Data = isHookaDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating a hookah detail : {ex.Message}");
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<HookahDetails>> ValidationRequest(CreateHookaDetailsCommand request)
        {
            try
            {
                // check if patient consumption method ID exists
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);
                if (isPatientConsumption is null)
                {
                    return new Response<HookahDetails>()
                    {
                        Succeeded = false,
                        Message = "Invalid patient consumption method Id"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when validating a hooka detail request : {ex.Message}");
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Somenthing went wrong"
                };
            }
            return null;
        }
    }
}
