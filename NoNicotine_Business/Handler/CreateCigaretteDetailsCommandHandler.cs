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
    public class CreateCigaretteDetailsCommandHandler : IRequestHandler<CreateCigaretteDetailsCommand, Response<CigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateCigaretteDetailsCommandHandler> _logger;
        public CreateCigaretteDetailsCommandHandler(AppDbContext context, ILogger<CreateCigaretteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<CigaretteDetails>> Handle(CreateCigaretteDetailsCommand request, CancellationToken cancellationToken)
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
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Invalid patient consumption method Id"
                };
            }

            var cigaretteDetails = new CigaretteDetails()
            {
                unitsPerBox = request.unitsPerBox,
                unitsPerDay = request.unitsPerDay,
                boxPrice = request.boxPrice,
                daysPerWeek = request.daysPerWeek,
                PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
            };

            await _context.CigaretteDetails.AddAsync(cigaretteDetails);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result < 1)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            // updates relationship with patient comsumption method
            patientConsumptionMethods.CigaretteDetailsId = cigaretteDetails.ID;
            _context.PatientConsumptionMethods.Update(patientConsumptionMethods);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response<CigaretteDetails>()
            {
                Succeeded = true,
                Data = cigaretteDetails
            };
        }

        private static Response<CigaretteDetails>? ValidateRequest(CreateCigaretteDetailsCommand request)
        {
            if (request.unitsPerDay <= 0)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Units per day must be greater than 0"
                };
            }

            if (request.daysPerWeek <= 0)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Days per week must be greater than 0"
                };
            }

            if (request.unitsPerBox <= 0)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Units per box must be greater than 0"
                };
            }

            if (request.boxPrice <= 0)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Box price must be greater than 0"
                };
            }

            return null;
        }
    }
}
