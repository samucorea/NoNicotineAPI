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
            try
            {
                var response = await ValidateRequest(request);
                if (request is not null)
                {
                    return response;
                }

                var isElectronicCigarette = await _context.ElectronicCigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync();
                
                if (request.unitsPerBox is not null)
                    isElectronicCigarette.unitsPerBox = (short)request.unitsPerBox;
                if (request.boxPrice is not null)
                    isElectronicCigarette.boxPrice = (decimal)request.boxPrice;
                if (request.cartridgeLifespan is not null)
                    isElectronicCigarette.cartridgeLifespan = (short)request.cartridgeLifespan;
               

                _context.ElectronicCigaretteDetails.Update(isElectronicCigarette);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<ElectronicCigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                _logger.LogInformation($"Electronic cigarette Detail with ID {isElectronicCigarette.ID} updated");
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = true,
                    Data = isElectronicCigarette
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating Electronic cigarette detail: {errMessage}", ex.Message);
                return new Response<ElectronicCigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<ElectronicCigaretteDetails>>? ValidateRequest(UpdateElectronicCigaretteDetailsCommand request)
        {
            var isElectronicCigarette = await _context.ElectronicCigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync();
            if (isElectronicCigarette is null)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Electronic cigarette Detail not found with specified id"
                };
            }
            return null;
        }
    }
}
