using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Update;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Update
{
    public class UpdateHookaDetailsCommandHandler : IRequestHandler<UpdateHookaDetailsCommand, Response<HookahDetails>>
    {
        private readonly ILogger<UpdateHookaDetailsCommandHandler> _logger;
        private readonly AppDbContext _context;

        public UpdateHookaDetailsCommandHandler(ILogger<UpdateHookaDetailsCommandHandler> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<Response<HookahDetails>> Handle(UpdateHookaDetailsCommand request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var hookahDetails = await _context.HookahDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync(cancellationToken);
            if (hookahDetails == null)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Hookah details was not found on patient"
                };
            }

            if (request.daysPerWeek is not null)
                hookahDetails.daysPerWeek = (short)request.daysPerWeek;
            if (request.setupPrice is not null)
                hookahDetails.setupPrice = (short)request.setupPrice;

            _context.HookahDetails.Update(hookahDetails);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation($"Hookah Detail with ID {hookahDetails.ID} updated");
            return new Response<HookahDetails>()
            {
                Succeeded = true,
                Data = hookahDetails
            };
        }

        private static Response<HookahDetails>? ValidateRequest(UpdateHookaDetailsCommand request)
        {
            if (request.PatientConsumptionMethodsId == string.Empty)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "You must specify a patient consumption methods id"
                };
            }

            return null;
        }
    }
}


