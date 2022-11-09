using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Queries;
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
    public class CreateCigarDetailsCommandHandler : IRequestHandler<CreateCigarDetailsCommand, Response<CigarDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateCigarDetailsCommandHandler> _logger;
        public CreateCigarDetailsCommandHandler(AppDbContext context, ILogger<CreateCigarDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<CigarDetails>> Handle(CreateCigarDetailsCommand request, CancellationToken cancellationToken)
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
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Invalid patient consumption method Id"
                };
            }

            var cigarDetails = new CigarDetails()
            {
                unitsPerBox = request.unitsPerBox,
                unitsPerDay = request.unitsPerDay,
                boxPrice = request.boxPrice,
                daysPerWeek = request.daysPerWeek,
                PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
            };

            await _context.CigarDetails.AddAsync(cigarDetails);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result < 1)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            // updates relationship with patient comsumption method
            patientConsumptionMethods.CigarDetailsId = cigarDetails.ID;
            _context.PatientConsumptionMethods.Update(patientConsumptionMethods);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response<CigarDetails>()
            {
                Succeeded = true,
                Data = cigarDetails
            };
        }

        private static Response<CigarDetails>? ValidateRequest(CreateCigarDetailsCommand request)
        {
            if(request.unitsPerDay <= 0)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Units per day must be greater than 0"
                };
            }

            if(request.daysPerWeek <= 0)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Days per week must be greater than 0"
                };
            }

            if(request.unitsPerBox <= 0)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Units per box must be greater than 0"
                };
            }

            if(request.boxPrice <= 0)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Box price must be greater than 0"
                };
            }

            return null;
        }
    }
}
