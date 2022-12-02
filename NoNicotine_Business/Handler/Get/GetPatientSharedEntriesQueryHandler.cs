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
    internal class GetPatientSharedEntriesQueryHandler : IRequestHandler<GetPatientSharedEntriesQuery, Response<List<SharedEntry>>>
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly AppDbContext _context;
        public GetPatientSharedEntriesQueryHandler(IEntryRepository entryRepository, IPatientRepository patientRepository, AppDbContext appDbContext)
        {
            _entryRepository = entryRepository;
            _patientRepository = patientRepository;
            _context = appDbContext;
        }

        public async Task<Response<List<SharedEntry>>> Handle(GetPatientSharedEntriesQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var therapist = await _context.Therapist.Where(therapist => therapist.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                return new Response<List<SharedEntry>>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist with specified id"
                };
            }

            var patient = await _patientRepository.GetTherapistPatientAsync(therapist.ID, request.PatientId, cancellationToken);
            if (patient == null)
            {
                return new Response<List<SharedEntry>>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist's patient"
                };
            }

            var entries = await _entryRepository.GetPatientSharedEntriesAsync(request.PatientId, cancellationToken);
            if (entries == null)
            {
                return new Response<List<SharedEntry>>
                {
                    Succeeded = false,
                    Message = "Could not find Patient's entries"
                };
            }

            return new Response<List<SharedEntry>>
            {
                Succeeded = true,
                Data = entries
            };

        }

        private static Response<List<SharedEntry>>? ValidateRequest(GetPatientSharedEntriesQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<List<SharedEntry>>
                {
                    Succeeded = false,
                    Message = "Missing Therapist ID"
                };
            }

            if (request.PatientId == string.Empty)
            {
                return new Response<List<SharedEntry>>
                {
                    Succeeded = false,
                    Message = "Missing Patient ID"
                };
            }

            return null;
        }
    }
}
