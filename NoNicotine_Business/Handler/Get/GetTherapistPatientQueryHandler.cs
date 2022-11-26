using MediatR;
using Microsoft.EntityFrameworkCore;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Get
{
    internal class GetTherapistPatientQueryHandler : IRequestHandler<GetTherapistPatientQuery, Response<TherapistPatient>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly AppDbContext _context;
        public GetTherapistPatientQueryHandler(AppDbContext context, IPatientRepository patientRepository)
        {
            _context = context;
            _patientRepository = patientRepository;
        }

        public async Task<Response<TherapistPatient>> Handle(GetTherapistPatientQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var therapist = await _context.Therapist.Where(therapist => therapist.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                return new Response<TherapistPatient>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist with specified id"
                };
            }

            var patient = await _patientRepository.GetTherapistPatientAsync(therapist.ID, request.PatientId, cancellationToken);
            if (patient == null)
            {
                return new Response<TherapistPatient>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist's patient"
                };
            }

            return new Response<TherapistPatient>
            {
                Succeeded = true,
                Data = patient
            };
        }

        private static Response<TherapistPatient>? ValidateRequest(GetTherapistPatientQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<TherapistPatient>
                {
                    Succeeded = false,
                    Message = "Missing Therapist ID"
                };
            }

            if(request.PatientId == string.Empty)
            {
                return new Response<TherapistPatient>
                {
                    Succeeded = false,
                    Message = "Missing Patient ID"
                };
            }

            return null;
        }
    }
}
