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
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patientCigarDetail = await _context.CigarDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionId).FirstOrDefaultAsync(cancellationToken);
            if (patientCigarDetail is null)
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
                Data = patientCigarDetail
            };
        }

        private static Response<CigarDetails>? ValidateRequest(GetCigarDetailsQuery request)
        {
            if(request.PatientConsumptionId == string.Empty)
            {
                return new Response<CigarDetails>
                {
                    Succeeded = false,
                    Message = "A patient consumption id must be specified"
                };
            }

            return null;
        }
    }
}
