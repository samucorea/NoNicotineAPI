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
    internal class UpdateCigaretteDetailsCommandHandler : IRequestHandler<UpdateCigaretteDetailsCommand, Response<CigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateCigaretteDetailsCommandHandler> _logger;
        public UpdateCigaretteDetailsCommandHandler(AppDbContext context, ILogger<UpdateCigaretteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<CigaretteDetails>> Handle(UpdateCigaretteDetailsCommand request, CancellationToken cancellationToken)
        {
            var currentCigaretteDetail = await _context.CigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.patientConsumptionId).FirstOrDefaultAsync();
            if (currentCigaretteDetail == null)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Cigarette Detail not found with specified id"
                };
            }

            if (request.unitsPerDay is not null)
                currentCigaretteDetail.unitsPerDay = (short)request.unitsPerDay;
            if (request.daysPerWeek is not null)
                currentCigaretteDetail.daysPerWeek = (short)request.daysPerWeek;
            if (request.unitsPerBox is not null)
                currentCigaretteDetail.unitsPerBox = (short)request.unitsPerBox;
            if (request.boxPrice is not null)
                currentCigaretteDetail.boxPrice = (short)request.boxPrice;

            _context.CigaretteDetails.Update(currentCigaretteDetail);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation("Cigarette Detail with ID {cigaretteDetailId} updated", currentCigaretteDetail.ID);
            return new Response<CigaretteDetails>()
            {
                Succeeded = true,
                Data = currentCigaretteDetail
            };
        }
    }
}

