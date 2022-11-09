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
    public class GetCigaretteDetailsQueryHandler : IRequestHandler<GetCigaretteDetailsQuery, Response<CigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetCigaretteDetailsQueryHandler> _logger;
        public GetCigaretteDetailsQueryHandler(AppDbContext context, ILogger<GetCigaretteDetailsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<CigaretteDetails>> Handle(GetCigaretteDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patientCigaretteDetail = await _context.CigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionId).FirstOrDefaultAsync(cancellationToken);
            if (patientCigaretteDetail is null)
            {
                return new Response<CigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Could not find cigarette detail with specified id"
                };
            }

            return new Response<CigaretteDetails>
            {
                Succeeded = true,
                Data = patientCigaretteDetail
            };
        }

        private static Response<CigaretteDetails>? ValidateRequest(GetCigaretteDetailsQuery request)
        {
            if(request.PatientConsumptionId == string.Empty)
            {
                return new Response<CigaretteDetails>
                {
                    Succeeded = false,
                    Message = "A patient consumption id must be specified"
                };
            }

            return null;
        }
    }
}
