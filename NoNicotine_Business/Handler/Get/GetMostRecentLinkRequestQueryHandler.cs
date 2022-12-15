using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoNicotine_Business.Handler.Get
{
    public class GetMostRecentLinkRequestQueryHandler : IRequestHandler<GetMostRecentLinkRequestQuery, Response<LinkRequest>>
    {

        private readonly AppDbContext _context;
        private readonly IPatientRepository _patientRepository;
        public GetMostRecentLinkRequestQueryHandler(AppDbContext context, IPatientRepository patientRepository)
        {
            _context = context;
            _patientRepository = patientRepository;
        }

        public async Task<Response<LinkRequest>> Handle(GetMostRecentLinkRequestQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _patientRepository.GetPatientByUserIdAsync(request.UserId, cancellationToken);
            if (patient == null)
            {
                return new Response<LinkRequest>
                {
                    Succeeded = false,
                    Message = "Could not find Patient with specified id"
                };
            }

            var mostRecentLinkRequest = await _context.LinkRequest.Include("Therapist")
            .OrderByDescending(linkRequest => linkRequest.CreatedAt)
            .Where(linkRequest => linkRequest.PatientId == patient.ID)
            .FirstOrDefaultAsync(cancellationToken);

            if(mostRecentLinkRequest == null){
              return new Response<LinkRequest>
              {
                Succeeded = false,
                Message = "Could not find link request associated with patient"
              };
            }

            return new Response<LinkRequest>
            {
              Succeeded = true,
              Data = mostRecentLinkRequest
            };
        }

        private static Response<LinkRequest>? ValidateRequest(GetMostRecentLinkRequestQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<LinkRequest>
                {
                    Succeeded = false,
                    Message = "Missing User Id"
                };
            }

            return null;
        }

 
  }
}
