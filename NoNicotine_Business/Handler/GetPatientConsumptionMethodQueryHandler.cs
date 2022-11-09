using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Queries;
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
    public class GetPatientConsumptionMethodQueryHandler : IRequestHandler<GetPatientConsumptionMethodQuery, Response<PatientConsumptionMethods>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetPatientConsumptionMethodQueryHandler> _logger;
        public GetPatientConsumptionMethodQueryHandler(AppDbContext context, ILogger<GetPatientConsumptionMethodQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<PatientConsumptionMethods>> Handle(GetPatientConsumptionMethodQuery request, CancellationToken cancellationToken)
        {
			try
			{
                // gets the patient consumption method
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionId);
                if (isPatientConsumption is null)
                {
                    return new Response<PatientConsumptionMethods>
                    {
                        Succeeded = false,
                        Message = "Could not find Patient consumption method with specified id"
                    };
                }

                return new Response<PatientConsumptionMethods>
                {
                    Succeeded = true,
                    Data = isPatientConsumption
                };
            }
			catch (Exception ex)
			{
                _logger.LogError("Error while getting patient consumption method: {errMessage}", ex.Message);
                return new Response<PatientConsumptionMethods>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
