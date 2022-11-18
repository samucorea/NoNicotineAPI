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
    internal class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, Response<List<Entry>>>
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IPatientRepository _patientRepository;
        public GetEntriesQueryHandler(IEntryRepository entryRepository, IPatientRepository patientRepository)
        {
            _entryRepository = entryRepository;
            _patientRepository = patientRepository;
        }

        public async Task<Response<List<Entry>>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _patientRepository.GetPatientByUserIdAsync(request.UserId, cancellationToken);
            if (patient == null)
            {
                return new Response<List<Entry>>
                {
                    Succeeded = false,
                    Message = "Could not find Patient with specified id"
                };
            }

            var entries = await _entryRepository.GetPatientEntriesAsync(request.UserId, cancellationToken);

            return new Response<List<Entry>>
            {
                Succeeded = true,
                Data = entries
            };

        }

        private static Response<List<Entry>>? ValidateRequest(GetEntriesQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<List<Entry>>
                {
                    Succeeded = false,
                    Message = "Missing Patient ID"
                };
            }

            return null;
        }
    }
}
