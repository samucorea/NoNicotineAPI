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
    internal class IndicateRelapseCommandHandler : IRequestHandler<IndicateRelapseCommand, Response<Patient>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IndicateRelapseCommandHandler> _logger;
        public IndicateRelapseCommandHandler(AppDbContext context, ILogger<IndicateRelapseCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<Patient>> Handle(IndicateRelapseCommand request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (patient == null)
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "Patient not found with specified id"
                };
            }

            patient.StartTime = request.RestartDate;
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

            _logger.LogInformation("Patient with ID {patientId} had a relapse", patient.ID);

            return new Response<Patient>()
            {
                Succeeded = true,
                Data = patient
            };
        }

        private static Response<Patient>? ValidateRequest(IndicateRelapseCommand request)
        {
            if (request == null || request.UserId == string.Empty)
            {
                return new Response<Patient>()
                {
                    Succeeded = false,
                    Message = "You must specify a user Id to update"
                };
            }

            return null;
        }
    }
}
