using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class GetHookahDetailsQueryHandler : IRequestHandler<GetHookahDetailsQuery, Response<HookahDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetHookahDetailsQueryHandler> _logger;
        public GetHookahDetailsQueryHandler(AppDbContext context, ILogger<GetHookahDetailsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<HookahDetails>> Handle(GetHookahDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // gets the Hookah Details
                var isHookahDetail = await _context.HookahDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumtionId).FirstOrDefaultAsync();
                if (isHookahDetail is null)
                {
                    return new Response<HookahDetails>
                    {
                        Succeeded = false,
                        Message = "Could not find hookah detail with specified id"
                    };
                }

                return new Response<HookahDetails>
                {
                    Succeeded = true,
                    Data = isHookahDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting hookah detail: {errMessage}", ex.Message);
                return new Response<HookahDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
