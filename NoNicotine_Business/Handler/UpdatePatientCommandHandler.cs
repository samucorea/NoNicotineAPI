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
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Response<Patient>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdatePatientCommandHandler> _logger;
        public UpdatePatientCommandHandler(AppDbContext context, ILogger<UpdatePatientCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<Patient>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if(response != null)
            {
                return response;
            }

            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (patient == null)
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "Patient not found with specified id"
                };
            }

            patient.BirthDate = request.BirthDate ?? patient.BirthDate;
            patient.Sex = request.Sex ?? patient.Sex;
            patient.Name = request.Name ?? patient.Name;

            _context.Patient.Update(patient);

            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation("Patient with ID {patientId} updated", patient.ID);

            return new Response<Patient>()
            {
                Succeeded = true,
                Data = patient
            };
        }

        private static Response<Patient>? ValidateRequest(UpdatePatientCommand request)
        {
            if(request == null || request.Id == string.Empty)
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "You must specify a user Id to update"
                };
            }

            if(request.Name != null && request.Name == string.Empty)
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "You can't update a patient with an empty name"
                };
            }

            if (request.Sex != null && (request.Sex != 'M' && request.Sex != 'F'))
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "You must specify a sex (M or F)"
                };
            }

            return null;
        }

      
    }
}
