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
    public class CreateCigarreteDetailsCommandHandler : IRequestHandler<CreateCigarreteDetailsCommand, Response<CigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateCigarreteDetailsCommandHandler> _logger;
        public CreateCigarreteDetailsCommandHandler(AppDbContext context, ILogger<CreateCigarreteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<CigaretteDetails>> Handle(CreateCigarreteDetailsCommand request, CancellationToken cancellationToken)
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


                var isCigarreteDetails = new CigaretteDetails()
                {
                    unitsPerBox = request.unitsPerBox,
                    unitsPerDay = request.unitsPerDay,
                    boxPrice = request.boxPrice,
                    daysPerWeek = request.daysPerWeek,
                    PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
                };

                await _context.CigaretteDetails.AddAsync(isCigarreteDetails);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<CigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                isPatientConsumption.CigaretteDetailsId = isCigarreteDetails.ID;
                _context.PatientConsumptionMethods.Update(isPatientConsumption);
                await _context.SaveChangesAsync();

                return new Response<CigaretteDetails>()
                {
                    Succeeded = true,
                    Data = isCigarreteDetails
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating a cigar detail : {ex.Message}");
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<CigaretteDetails>> ValidationRequest(CreateCigarreteDetailsCommand request)
        {
            try
            {
                // check if patient consumption method ID exists
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);
                if (isPatientConsumption is null)
                {
                    return new Response<CigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Invalid patient consumption method Id"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when validating a cigarette detail request : {ex.Message}");
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Somenthing went wrong"
                };
            }
            return null;
        }
    }
}
