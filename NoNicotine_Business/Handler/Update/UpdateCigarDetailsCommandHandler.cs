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
            var currentCigarDetail = await _context.CigarDetails.Where(x => x.PatientConsumptionMethodsId == request.patientConsumptionId).FirstOrDefaultAsync();
            if (currentCigarDetail == null)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Cigar Detail not found with specified id"
                };
            }

            if (request.unitsPerDay is not null)
                currentCigarDetail.unitsPerDay = (short)request.unitsPerDay;
            if (request.daysPerWeek is not null)
                currentCigarDetail.daysPerWeek = (short)request.daysPerWeek;
            if (request.unitsPerBox is not null)
                currentCigarDetail.unitsPerBox = (short)request.unitsPerBox;
            if (request.boxPrice is not null)
                currentCigarDetail.boxPrice = (short)request.boxPrice;

            _context.CigarDetails.Update(currentCigarDetail);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation("Cigar Detail with ID {cigarDetailId} updated", currentCigarDetail.ID);
            return new Response<CigarDetails>()
            {
                Succeeded = true,
                Data = currentCigarDetail
            };
        }


    }
}
