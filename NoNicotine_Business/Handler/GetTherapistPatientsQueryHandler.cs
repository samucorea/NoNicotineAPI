using MediatR;
using Microsoft.EntityFrameworkCore;
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
    internal class GetTherapistPatientsQueryHandler : IRequestHandler<GetTherapistPatientsQuery, Response<List<Patient>>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly AppDbContext _context;
        public GetTherapistPatientsQueryHandler(AppDbContext context, IPatientRepository patientRepository)
        {
            _context = context;
            _patientRepository = patientRepository;
        }

        public async Task<Response<List<Patient>>> Handle(GetTherapistPatientsQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }
            
            var therapist = await _context.Therapist.Where(therapist => therapist.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                return new Response<List<Patient>>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist with specified id"
                };
            }

            var patients = await _patientRepository.GetTherapistPatientsAsync(request.UserId, cancellationToken);

            return new Response<List<Patient>>
            {
                Succeeded = true,
                Data = patients
            };

        }

        private static Response<List<Patient>>? ValidateRequest(GetTherapistPatientsQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<List<Patient>>
                {
                    Succeeded = false,
                    Message = "Missing Therapist ID"
                };
            }

            return null;
        }
    }
}
