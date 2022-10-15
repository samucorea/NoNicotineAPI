using MediatR;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler
{
    public class CreateEntryCommandHandler : IRequestHandler<CreateEntryCommand, Response<Entry>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IEntryRepository _entryRepository;
        public CreateEntryCommandHandler(IPatientRepository patientRepository, IEntryRepository entryRepository)
        {
            _patientRepository = patientRepository;
            _entryRepository = entryRepository;

        }
        public async Task<Response<Entry>> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _patientRepository.GetPatientByUserIdAsync(request.UserId, cancellationToken);
            if (patient == null)
            {
                return new Response<Entry>()
                {
                    Succeeded = false,
                    Message = "No patient associated with user id found"
                };
            }

            var newEntry = new Entry()
            {
                PatientId = patient.ID,
                Symptoms = MapListToString(request.Symptoms),
                Feelings = MapListToString(request.Feelings),
                Message = request.Message,
                TherapistAllowed = request.TherapistAllowed,
            };

            var succeeded = await _entryRepository.CreateEntryAsync(newEntry, cancellationToken);

            if(!succeeded)
            {
                return new Response<Entry>()
                {
                    Succeeded = false,
                    Message = "Something went wrong creating entry"
                };
            }

            return new Response<Entry>()
            {
                Data = newEntry,
                Succeeded = true
            };
        }

        private static string MapListToString(List<string> list)
        { 
            return string.Join(',', list);
        }

        private static Response<Entry>? ValidateRequest(CreateEntryCommand request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<Entry>()
                {
                    Succeeded = false,
                    Message = "Missing user id"
                };
            }

            if(request.Symptoms.Count < 1)
            {
                return new Response<Entry>()
                {
                    Succeeded = false,
                    Message = "Missing at least 1 symptom in patient entry"
                };
            }

            if (request.Feelings.Count < 1)
            {
                return new Response<Entry>()
                {
                    Succeeded = false,
                    Message = "Missing at least 1 feeling in patient entry"
                };
            }

            foreach(var feeling in request.Feelings)
            {
                if (!Feelings.Values.ContainsKey(feeling))
                {
                    return new Response<Entry>()
                    {
                        Succeeded = false,
                        Message = $"Feeling {feeling} not currently valid"
                    };
                }
            }

            foreach (var symptom in request.Symptoms)
            {
                if (!Symptoms.Values.ContainsKey(symptom))
                {
                    return new Response<Entry>()
                    {
                        Succeeded = false,
                        Message = $"Symptom {symptom} not currently valid"
                    };
                }
            }

         

            return null;
        }
    }
}
