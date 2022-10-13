using MediatR;
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
    public class UpdatePatientConsumptionMethodCommandHandler : IRequestHandler<UpdatePatientConsumptionMethodCommand, Response<PatientConsumptionMethods>>
    {
        private readonly ILogger<UpdatePatientConsumptionMethodCommandHandler> _logger;
        private readonly AppDbContext _context;

        public UpdatePatientConsumptionMethodCommandHandler(ILogger<UpdatePatientConsumptionMethodCommandHandler> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        public async Task<Response<PatientConsumptionMethods>> Handle(UpdatePatientConsumptionMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientMethodId);
                if (isPatientConsumption is not null)
                {
                    var isValidation = await ValidateRequest(request);
                    if (isValidation != null)
                    {
                        return isValidation;
                    }

                    if (request.CigaretteDetailsId is not null)
                        isPatientConsumption.CigaretteDetailsId = request.CigaretteDetailsId;
                    if (request.CigarDetailsId is not null)
                        isPatientConsumption.CigarDetailsId = request.CigarDetailsId;
                    if (request.ElectronicCigaretteDetailsId is not null)
                        isPatientConsumption.ElectronicCigaretteDetailsId = request.ElectronicCigaretteDetailsId;
                    if (request.HookahDetailsId is not null)
                        isPatientConsumption.HookahDetailsId = request.HookahDetailsId;

                    _context.PatientConsumptionMethods.Update(isPatientConsumption);
                    var result = await _context.SaveChangesAsync();

                    if (result < 1)
                    {
                        return new Response<PatientConsumptionMethods>()
                        {
                            Succeeded = false,
                            Message = "Something went wrong"
                        };
                    }
                    _logger.LogInformation($"Patient Consumption Method with ID {isPatientConsumption.ID} updated");
                    return new Response<PatientConsumptionMethods>()
                    {
                        Succeeded = true,
                        Data = isPatientConsumption
                    };
                }

                return new Response<PatientConsumptionMethods>
                {
                    Succeeded = false,
                    Message = "Invalid ID"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating patient consumption method: {errMessage}", ex.Message);
                return new Response<PatientConsumptionMethods>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<PatientConsumptionMethods>>? ValidateRequest(UpdatePatientConsumptionMethodCommand request)
        {
            try
            {
                if (request.CigaretteDetailsId is not null)
                {
                    var isCigarreteDetails = await _context.CigaretteDetails.FindAsync(request.CigaretteDetailsId);
                    if (isCigarreteDetails is null)
                    {
                        return new Response<PatientConsumptionMethods>()
                        {
                            Succeeded = false,
                            Message = "Invalid Cigarrete Details Id"
                        };
                    }
                }
                if (request.ElectronicCigaretteDetailsId is not null)
                {
                    var isElectronicCigarrete = await _context.ElectronicCigaretteDetails.FindAsync(request.ElectronicCigaretteDetailsId);
                    if (isElectronicCigarrete is null)
                    {
                        return new Response<PatientConsumptionMethods>()
                        {
                            Succeeded = false,
                            Message = "Invalid Electronic Cigarrete Details Id"
                        };
                    }
                }
                if (request.CigarDetailsId is not null)
                {
                    var isCigarDetails = await _context.CigarDetails.FindAsync(request.CigarDetailsId);
                    if (isCigarDetails is null)
                    {
                        return new Response<PatientConsumptionMethods>()
                        {
                            Succeeded = false,
                            Message = "Invalid Cigar Details Id"
                        };
                    }
                }
                if (request.HookahDetailsId is not null)
                {
                    var isHookaDetails = await _context.HookahDetails.FindAsync(request.HookahDetailsId);
                    if (isHookaDetails is null)
                    {
                        return new Response<PatientConsumptionMethods>()
                        {
                            Succeeded = false,
                            Message = "Invalid Hooka Details Id"
                        };
                    }
                }
            }
            catch (Exception)
            {
                return new Response<PatientConsumptionMethods>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
            return null;
        }
    }
}
