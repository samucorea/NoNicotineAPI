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
    public class UpdateElectronicCigarreteDetailsCommandHandler : IRequestHandler<UpdateElectronicCigarreteDetailsCommand, Response<ElectronicCigaretteDetails>>
    {
        private readonly ILogger<UpdateElectronicCigarreteDetailsCommandHandler> _logger;
        private readonly AppDbContext _context;

        public UpdateElectronicCigarreteDetailsCommandHandler(ILogger<UpdateElectronicCigarreteDetailsCommandHandler> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<Response<ElectronicCigaretteDetails>> Handle(UpdateElectronicCigarreteDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await ValidateRequest(request);
                if (request is not null)
                {
                    return response;
                }

                var isElectronicCigarrete = await _context.ElectronicCigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync();
                
                if (request.unitsPerBox is not null)
                    isElectronicCigarrete.unitsPerBox = (short)request.unitsPerBox;
                if (request.boxPrice is not null)
                    isElectronicCigarrete.boxPrice = (decimal)request.boxPrice;
                if (request.cartridgeLifespan is not null)
                    isElectronicCigarrete.cartridgeLifespan = (short)request.cartridgeLifespan;
               

                _context.ElectronicCigaretteDetails.Update(isElectronicCigarrete);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<ElectronicCigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                _logger.LogInformation($"Electronic cigarrete Detail with ID {isElectronicCigarrete.ID} updated");
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = true,
                    Data = isElectronicCigarrete
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating Electronic cigarrete detail: {errMessage}", ex.Message);
                return new Response<ElectronicCigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<ElectronicCigaretteDetails>>? ValidateRequest(UpdateElectronicCigarreteDetailsCommand request)
        {
            var isElectronicCigarrete = await _context.ElectronicCigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientConsumptionMethodsId).FirstOrDefaultAsync();
            if (isElectronicCigarrete is null)
            {
                return new Response<ElectronicCigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Electronic cigarrete Detail not found with specified id"
                };
            }
            return null;
        }
    }
}
