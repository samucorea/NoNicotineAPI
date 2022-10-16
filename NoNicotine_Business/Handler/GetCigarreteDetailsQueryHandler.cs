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
    public class GetCigarreteDetailsQueryHandler : IRequestHandler<GetCigarreteDetailsQuery, Response<CigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetCigarreteDetailsQueryHandler> _logger;
        public GetCigarreteDetailsQueryHandler(AppDbContext context, ILogger<GetCigarreteDetailsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<CigaretteDetails>> Handle(GetCigarreteDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // gets the Cigarette Details
                var isCigarDetail = await _context.CigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumtionId).FirstOrDefaultAsync();
                if (isCigarDetail is null)
                {
                    return new Response<CigaretteDetails>
                    {
                        Succeeded = false,
                        Message = "Could not find cigarrete detail with specified id"
                    };
                }

                return new Response<CigaretteDetails>
                {
                    Succeeded = true,
                    Data = isCigarDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting cigarrete detail: {errMessage}", ex.Message);
                return new Response<CigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
