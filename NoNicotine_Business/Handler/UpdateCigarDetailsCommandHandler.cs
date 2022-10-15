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
    public class UpdateCigarDetailsCommandHandler : IRequestHandler<UpdateCigarDetailsCommand, Response<CigarDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateCigarDetailsCommandHandler> _logger;
        public UpdateCigarDetailsCommandHandler(AppDbContext context, ILogger<UpdateCigarDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<CigarDetails>> Handle(UpdateCigarDetailsCommand request, CancellationToken cancellationToken)
        {
			try
			{
                var response = await ValidateRequest(request);
                if (request is not null)
                {
                    return response;
                }
                var isCigarDetail = await _context.CigarDetails.Where(x => x.PatientConsumptionMethodsId == request.patientConsumptionId).FirstOrDefaultAsync();

                if (request.unitsPerDay is not null)
                    isCigarDetail.unitsPerDay = (short)request.unitsPerDay;
                if (request.daysPerWeek is not null)
                    isCigarDetail.daysPerWeek = (short)request.daysPerWeek;
                if (request.unitsPerBox is not null)
                    isCigarDetail.unitsPerBox = (short)request.unitsPerBox;
                if (request.boxPrice is not null)
                    isCigarDetail.boxPrice = (short)request.boxPrice;

                _context.CigarDetails.Update(isCigarDetail);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<CigarDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                _logger.LogInformation($"Cigar Detail with ID {isCigarDetail.ID} updated");
                return new Response<CigarDetails>()
                {
                    Succeeded = true,
                    Data = isCigarDetail
                };
            }
			catch (Exception ex)
			{
                _logger.LogError("Error updating cigar detail: {errMessage}", ex.Message);
                return new Response<CigarDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<CigarDetails>>? ValidateRequest(UpdateCigarDetailsCommand request)
        {
            var isCigarDetail = await _context.CigarDetails.Where(x=>x.PatientConsumptionMethodsId == request.patientConsumptionId).FirstOrDefaultAsync();
            if (isCigarDetail is null)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Cigar Detail not found with specified id"
                };
            }
            return null;
        }
    }
}
