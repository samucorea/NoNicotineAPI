using MediatR;
using Microsoft.EntityFrameworkCore;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Value_Objects;
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
    internal class GetTherapistPatientsQueryHandler : IRequestHandler<GetTherapistPatientsQuery, Response<List<TherapistPatient>>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly AppDbContext _context;
        public GetTherapistPatientsQueryHandler(AppDbContext context, IPatientRepository patientRepository)
        {
            _context = context;
            _patientRepository = patientRepository;
        }

        public async Task<Response<List<TherapistPatient>>> Handle(GetTherapistPatientsQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var therapist = await _context.Therapist.Where(therapist => therapist.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                return new Response<List<TherapistPatient>>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist with specified id"
                };
            }

            var patients = await _patientRepository.GetTherapistPatientsAsync(therapist.ID, cancellationToken);

            return new Response<List<TherapistPatient>>
            {
                Succeeded = true,
                Data = patients
            };

        }

        private static Response<List<TherapistPatient>>? ValidateRequest(GetTherapistPatientsQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<List<TherapistPatient>>
                {
                    Succeeded = false,
                    Message = "Missing Therapist ID"
                };
            }

            return null;
        }
    }
}
