using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Create
{
    public class CreateElectronicCigarreteDetailsCommandHandler : IRequestHandler<CreateElectronicCigarreteDetailsCommand, Response<ElectronicCigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateElectronicCigarreteDetailsCommandHandler> _logger;
        public CreateElectronicCigarreteDetailsCommandHandler(AppDbContext context, ILogger<CreateElectronicCigarreteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<ElectronicCigaretteDetails>> Handle(CreateElectronicCigarreteDetailsCommand request, CancellationToken cancellationToken)
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

                var isElectronicCigaretteDetails = new ElectronicCigaretteDetails()
                {
                    boxPrice = request.boxPrice,
                    cartridgeLifespan = request.cartridgeLifespan,
                    unitsPerBox = request.unitsPerBox,
                    PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
                };

                await _context.ElectronicCigaretteDetails.AddAsync(isElectronicCigaretteDetails);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<ElectronicCigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }
                // updates relationship with patient comsumption method
                isPatientConsumption.ElectronicCigaretteDetailsId = isElectronicCigaretteDetails.ID;
                _context.PatientConsumptionMethods.Update(isPatientConsumption);
                await _context.SaveChangesAsync();

                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = true,
                    Data = isElectronicCigaretteDetails
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating a electronic cigar detail : {ex.Message}");
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<ElectronicCigaretteDetails>> ValidationRequest(CreateElectronicCigarreteDetailsCommand request)
        {
            try
            {
                // check if patient consumption method ID exists
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);
                if (isPatientConsumption is null)
                {
                    return new Response<ElectronicCigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Invalid patient consumption method Id"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when validating a electronic cigarette detail request : {ex.Message}");
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Somenthing went wrong"
                };
            }
            return null;
        }
    }
}
