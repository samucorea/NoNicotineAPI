using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Create
{
    public class CreatePatientConsumptionMethodCommandHandler : IRequestHandler<CreatePatientConsumptionMethodCommand, Response<PatientConsumptionMethods>>
    {
        private readonly ILogger<CreatePatientConsumptionMethodCommand> _logger;
        private readonly AppDbContext _context;

        public CreatePatientConsumptionMethodCommandHandler(ILogger<CreatePatientConsumptionMethodCommand> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        public async Task<Response<PatientConsumptionMethods>> Handle(CreatePatientConsumptionMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // verify if patient exists
                var isPatient = await _context.Patient.FindAsync(request.PatientId);
                if (isPatient is not null)
                {
                    var isPatientMethod = new PatientConsumptionMethods()
                    {
                        PatientId = request.PatientId
                    };
                    await _context.PatientConsumptionMethods.AddAsync(isPatientMethod);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                    {
                        return new Response<PatientConsumptionMethods>
                        {
                            Succeeded = true,
                            Data = isPatientMethod
                        };
                    }
                    else
                    {
                        _logger.LogError("Saving changes when creating patient consumption method");
                        return new Response<PatientConsumptionMethods>
                        {
                            Succeeded = false,
                            Message = "Something went wrong"
                        };
                    }
                }
                return new Response<PatientConsumptionMethods>
                {
                    Succeeded = false,
                    Message = "Invalid Patient ID"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating patient: {errMessage}", ex.Message);
                return new Response<PatientConsumptionMethods>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
