using MediatR;
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


namespace NoNicotine_Business.Handler
{
    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, Response<Patient>>
    {

        private readonly AppDbContext _context;
        public GetPatientQueryHandler(AppDbContext context)
        {
            _context = context;
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
            if (request.Id == string.Empty)
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
