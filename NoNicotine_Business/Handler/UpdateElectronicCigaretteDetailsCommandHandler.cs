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
    public class UpdateElectronicCigaretteDetailsCommandHandler : IRequestHandler<UpdateElectronicCigaretteDetailsCommand, Response<ElectronicCigaretteDetails>>
    {
        private readonly ILogger<UpdateElectronicCigaretteDetailsCommandHandler> _logger;
        private readonly AppDbContext _context;

        public UpdateElectronicCigaretteDetailsCommandHandler(ILogger<UpdateElectronicCigaretteDetailsCommandHandler> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<Response<ElectronicCigaretteDetails>> Handle(UpdateElectronicCigaretteDetailsCommand request, CancellationToken cancellationToken)
        {
            var currentElectronicCigaretteDetail = await _context.ElectronicCigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.patientConsumptionId).FirstOrDefaultAsync();
            if (currentElectronicCigaretteDetail == null)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Electronic Cigarette Detail not found with specified id"
                };
            }

            if (request.unitsPerBox is not null)
                currentElectronicCigaretteDetail.unitsPerBox = (short)request.unitsPerBox;
            if (request.boxPrice is not null)
                currentElectronicCigaretteDetail.boxPrice = (decimal)request.boxPrice;
            if (request.cartridgeLifespan is not null)
                currentElectronicCigaretteDetail.cartridgeLifespan = (short)request.cartridgeLifespan;

            _context.ElectronicCigaretteDetails.Update(currentElectronicCigaretteDetail);
            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation("Electronic Cigarette Detail with ID {electronicCigaretteDetailId} updated", currentElectronicCigaretteDetail.ID);
            return new Response<ElectronicCigaretteDetails>()
            {
                Succeeded = true,
                Data = currentElectronicCigaretteDetail
            };
        }
    }
}
