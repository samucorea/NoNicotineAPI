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
    public class GetCigarDetailsQueryHandler : IRequestHandler<GetCigarDetailsQuery, Response<CigarDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetCigarDetailsQueryHandler> _logger;
        public GetCigarDetailsQueryHandler(AppDbContext context, ILogger<GetCigarDetailsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<CigarDetails>> Handle(GetCigarDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // gets the patient consumption method
                var isCigarDetail = await _context.CigarDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumtionId).FirstOrDefaultAsync();
                if (isCigarDetail is null)
                {
                    return new Response<CigarDetails>
                    {
                        Succeeded = false,
                        Message = "Could not find cigar detail with specified id"
                    };
                }

                return new Response<CigarDetails>
                {
                    Succeeded = true,
                    Data = isCigarDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting cigar detail: {errMessage}", ex.Message);
                return new Response<CigarDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
