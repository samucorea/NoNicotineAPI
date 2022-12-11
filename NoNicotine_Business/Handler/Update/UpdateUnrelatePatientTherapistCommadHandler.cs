using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Update;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Update
{
    public class UpdateUnrelatePatientPatientCommadHandler : IRequestHandler<UpdateUnrelatePatientTherapistCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateUnrelatePatientPatientCommadHandler> _logger;
        public UpdateUnrelatePatientPatientCommadHandler(AppDbContext context, ILogger<UpdateUnrelatePatientPatientCommadHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(UpdateUnrelatePatientTherapistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = ValidateRequest(request);
                if (response != null)
                {
                    return response;
                }

                Patient? patient = null;

                if(request.Role == "patient"){
                    patient = await _context.Patient.Where(x => x.ID == request.PatientId && x.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
                }
                else if(request.Role == "therapist"){
                    var therapistAssociated = await _context.Therapist.FirstOrDefaultAsync(therapist => therapist.IdentityUserId == request.UserId);
                    if(therapistAssociated == null){
                        return new Response<bool>{
                            Succeeded = false,
                            Message = "No therapist found"
                        };
                    }
                    patient = await _context.Patient.FirstOrDefaultAsync(patient => patient.TherapistId == therapistAssociated.ID);
                }
                
                if (patient == null)
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "Patient with specified ID not found",
                        Data = false
                    };
                }

                patient.TherapistId = null;

                _context.Patient.Update(patient);

                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong",
                        Data = false
                    };
                }

                _logger.LogInformation($"Patient with ID {request.PatientId} updated");

                return new Response<bool>()
                {
                    Succeeded = true,
                    Data = true
                };

            }
            catch (Exception)
            {
                return new Response<bool>
                {
                    Succeeded = false,
                    Message = "Something went wrong",
                    Data = false
                };
            }
        }

        private static Response<bool>? ValidateRequest(UpdateUnrelatePatientTherapistCommand request)
        {
            if (request == null || request.PatientId == string.Empty || request.UserId == string.Empty)
            {
                return new Response<bool>()
                {
                    Succeeded = false,
                    Message = "You must specify a user ID to update",
                    Data = false
                };
            }
            return null;
        }

    }
}
