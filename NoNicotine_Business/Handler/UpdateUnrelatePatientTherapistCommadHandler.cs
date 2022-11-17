using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
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

                var isPatient = await _context.Patient.Where(x => x.ID == request.PatientId && x.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
                if (isPatient == null)
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "Patient with specified ID not found",
                        Data = false
                    };
                }

                isPatient.TherapistId = null;

                _context.Patient.Update(isPatient);

                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong",
                        Data  =false
                    };
                }

                _logger.LogInformation($"Patient with ID {request.PatientId} updated");

                return new Response<bool>()
                {
                    Succeeded = true,
                    Data = true
                };

            }
            catch (Exception )
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
