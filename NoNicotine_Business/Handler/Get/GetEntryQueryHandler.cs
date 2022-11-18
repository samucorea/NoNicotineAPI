using MediatR;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Get
{
    public class GetEntryQueryHandler : IRequestHandler<GetEntryQuery, Response<Entry>>
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IPatientRepository _patientRepository;
        public GetEntryQueryHandler(IEntryRepository entryRepository, IPatientRepository patientRepository)
        {
            _entryRepository = entryRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Response<Entry>> Handle(GetEntryQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _patientRepository.GetPatientByUserIdAsync(request.UserId, cancellationToken);
            if (patient == null)
            {
                return new Response<Entry>
                {
                    Succeeded = false,
                    Message = "Could not find Patient with specified id"
                };
            }

            var entry = await _entryRepository.GetPatientEntryByIdAsync(request.UserId, request.EntryId, cancellationToken);
            if (entry == null)
            {
                return new Response<Entry>
                {
                    Succeeded = false,
                    Message = "Could not find Patient Entry with specified id"
                };
            }

            return new Response<Entry>
            {
                Succeeded = true,
                Data = entry
            };

        }

        private static Response<Entry>? ValidateRequest(GetEntryQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<Entry>
                {
                    Succeeded = false,
                    Message = "Missing Patient ID"
                };
            }

            if (request.EntryId == string.Empty)
            {
                return new Response<Entry>
                {
                    Succeeded = false,
                    Message = "Missing Entry ID"
                };
            }

            return null;
        }
    }
}
