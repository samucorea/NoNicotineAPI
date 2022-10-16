using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetElectronicCigarreteDetailQueryHandler : IRequestHandler<GetElectronicCigarreteDetailQuery, Response<ElectronicCigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetElectronicCigarreteDetailQueryHandler> _logger;
        public GetElectronicCigarreteDetailQueryHandler(AppDbContext context, ILogger<GetElectronicCigarreteDetailQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<ElectronicCigaretteDetails>> Handle(GetElectronicCigarreteDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // gets the patient consumption method
                var isElectronicCigarreteDetail = await _context.ElectronicCigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumtionId).FirstOrDefaultAsync();
                if (isElectronicCigarreteDetail is null)
                {
                    return new Response<ElectronicCigaretteDetails>
                    {
                        Succeeded = false,
                        Message = "Could not find electronic cigar detail with specified id"
                    };
                }

                return new Response<ElectronicCigaretteDetails>
                {
                    Succeeded = true,
                    Data = isElectronicCigarreteDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting electronic cigar detail detail: {errMessage}", ex.Message);
                return new Response<ElectronicCigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
