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
            // validation 
            var response = ValidateRequest(request);
            if (response is not null)
            {
                return response;
            }
            // patient consumption method
            var patientConsumptionMethods = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);
            if (patientConsumptionMethods == null)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Patient consumption method not found"
                };
            }

            var hookahDetails = new HookahDetails()
            {
                setupPrice = request.setupPrice,
                daysPerWeek = request.daysPerWeek,
                PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
            };

            await _context.HookahDetails.AddAsync(hookahDetails);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result < 1)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            patientConsumptionMethods.HookahDetailsId = hookahDetails.ID;
            _context.PatientConsumptionMethods.Update(patientConsumptionMethods);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response<HookahDetails>()
            {
                Succeeded = true,
                Data = hookahDetails
            };
        }

        private static Response<HookahDetails>? ValidateRequest(CreateHookaDetailsCommand request)
        {
            if(request.daysPerWeek <= 0)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Days per week must be greater than 0"
                };
            }

            if(request.setupPrice <= 0)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Setup price must be greater than 0"
                };
            }
     
            return null;
        }
    }
}
