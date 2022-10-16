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
            try
            {
                var response = await ValidateRequest(request);
                if (request is not null)
                {
                    return response;
                }

                var isHookaDetails = await _context.HookahDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync(); ;
                if (request.daysPerWeek is not null)
                    isHookaDetails.daysPerWeek = (short)request.daysPerWeek;
                if (request.setupPrice is not null)
                    isHookaDetails.setupPrice = (short)request.setupPrice;

                _context.HookahDetails.Update(isHookaDetails);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<HookahDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                _logger.LogInformation($"Hookah Detail with ID {isHookaDetails.ID} updated");
                return new Response<HookahDetails>()
                {
                    Succeeded = true,
                    Data = isHookaDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating hooka detail: {errMessage}", ex.Message);
                return new Response<HookahDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<HookahDetails>>? ValidateRequest(UpdateHookaDetailsCommand request)
        {
            var isHookaDetails = await _context.HookahDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync();
            if (isHookaDetails is null)
            {
                return new Response<HookahDetails>()
                {
                    Succeeded = false,
                    Message = "Hookah Detail not found with specified id"
                };
            }
            return null;
        }
    }
}


