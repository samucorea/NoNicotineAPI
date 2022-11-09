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
    public class CreateElectronicCigaretteDetailsCommandHandler : IRequestHandler<CreateElectronicCigaretteDetailsCommand, Response<ElectronicCigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateElectronicCigaretteDetailsCommandHandler> _logger;
        public CreateElectronicCigaretteDetailsCommandHandler(AppDbContext context, ILogger<CreateElectronicCigaretteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<ElectronicCigaretteDetails>> Handle(CreateElectronicCigaretteDetailsCommand request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            // check if patient consumption method ID exists
            var patientConsumptionMethods = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);
            if (patientConsumptionMethods is null)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Invalid patient consumption method Id"
                };
            }

            var electronicCigaretteDetails = new ElectronicCigaretteDetails()
            {
                cartridgeLifespan = request.cartridgeLifespan,
                unitsPerBox = request.unitsPerBox,
                boxPrice = request.boxPrice,
                PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
            };

            await _context.ElectronicCigaretteDetails.AddAsync(electronicCigaretteDetails);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result < 1)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            // updates relationship with patient comsumption method
            patientConsumptionMethods.ElectronicCigaretteDetailsId = electronicCigaretteDetails.ID;
            _context.PatientConsumptionMethods.Update(patientConsumptionMethods);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response<ElectronicCigaretteDetails>()
            {
                Succeeded = true,
                Data = electronicCigaretteDetails
            };
        }

        private static Response<ElectronicCigaretteDetails>? ValidateRequest(CreateElectronicCigaretteDetailsCommand request)
        {
            if (request.cartridgeLifespan <= 0)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Cartridge lifespan must be greater than 0"
                };
            }

            if (request.unitsPerBox <= 0)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Units per box must be greater than 0"
                };
            }

            if (request.boxPrice <= 0)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Box price must be greater than 0"
                };
            }

            return null;
        }
    }
}
