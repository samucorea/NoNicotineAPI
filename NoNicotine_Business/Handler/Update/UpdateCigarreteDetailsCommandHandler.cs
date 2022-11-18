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
    internal class UpdateCigarreteDetailsCommandHandler : IRequestHandler<UpdateCigarreteDetailsCommand, Response<CigaretteDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateCigarreteDetailsCommandHandler> _logger;
        public UpdateCigarreteDetailsCommandHandler(AppDbContext context, ILogger<UpdateCigarreteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<CigaretteDetails>> Handle(UpdateCigarreteDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await ValidateRequest(request);
                if (request is not null)
                {
                    return response;
                }
                var isCigarreteDetail = await _context.CigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientMethodId).FirstOrDefaultAsync(); ;

                if (request.unitsPerDay is not null)
                    isCigarreteDetail.unitsPerDay = (short)request.unitsPerDay;
                if (request.daysPerWeek is not null)
                    isCigarreteDetail.daysPerWeek = (short)request.daysPerWeek;
                if (request.unitsPerBox is not null)
                    isCigarreteDetail.unitsPerBox = (short)request.unitsPerBox;
                if (request.boxPrice is not null)
                    isCigarreteDetail.boxPrice = (short)request.boxPrice;

                _context.CigaretteDetails.Update(isCigarreteDetail);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<CigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                _logger.LogInformation($"Cigarrete Detail with ID {isCigarreteDetail.ID} updated");
                return new Response<CigaretteDetails>()
                {
                    Succeeded = true,
                    Data = isCigarreteDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating cigarrete detail: {errMessage}", ex.Message);
                return new Response<CigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<CigaretteDetails>>? ValidateRequest(UpdateCigarreteDetailsCommand request)
        {
            var isCigarreteDetail = await _context.CigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientMethodId).FirstOrDefaultAsync();
            if (isCigarreteDetail is null)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Cigarrete Detail not found with specified id"
                };
            }
            return null;
        }
    }
}

