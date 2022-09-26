using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotin_Business.Queries;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoNicotin_Business.Handler
{
    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, Response<Patient>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<GetPatientQueryHandler> _logger;
        public GetPatientQueryHandler(AppDbContext context, ILogger<GetPatientQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<Patient>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _context.Patient.FindAsync(request.Id);
            if (patient == null)
            {
                return new Response<Patient>
                {
                    Succeeded = false,
                    Message = "Could not find Patient with specified id"
                };
            }

            return new Response<Patient>
            {
                Succeeded = true,
                Data = patient
            };

        }

        private static Response<Patient>? ValidateRequest(GetPatientQuery request)
        {
            if (request.Id == null || request.Id.Length == 0)
            {
                return new Response<Patient>
                {
                    Succeeded = false,
                    Message = "Missing Patient Id"
                };
            }

            return null;
        }

    }
}
