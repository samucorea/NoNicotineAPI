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
            try
            {
                var response = await ValidateRequest(request);
                if (request is not null)
                {
                    return response;
                }
                var isCigaretteDetail = await _context.CigaretteDetails.Where(x => x.PatientConsumptionMethodsId == request.PatientMethodId).FirstOrDefaultAsync(); ;

                if (request.unitsPerDay is not null)
                    isCigaretteDetail.unitsPerDay = (short)request.unitsPerDay;
                if (request.daysPerWeek is not null)
                    isCigaretteDetail.daysPerWeek = (short)request.daysPerWeek;
                if (request.unitsPerBox is not null)
                    isCigaretteDetail.unitsPerBox = (short)request.unitsPerBox;
                if (request.boxPrice is not null)
                    isCigaretteDetail.boxPrice = (short)request.boxPrice;

                _context.CigaretteDetails.Update(isCigaretteDetail);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<CigaretteDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                _logger.LogInformation($"Cigarette Detail with ID {isCigaretteDetail.ID} updated");
                return new Response<CigaretteDetails>()
                {
                    Succeeded = true,
                    Data = isCigaretteDetail
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating cigarette detail: {errMessage}", ex.Message);
                return new Response<CigaretteDetails>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<CigaretteDetails>>? ValidateRequest(UpdateCigaretteDetailsCommand request)
        {
            var isCigaretteDetail = await _context.CigaretteDetails.Where( x=>x.PatientConsumptionMethodsId == request.PatientMethodId).FirstOrDefaultAsync();
            if (isCigaretteDetail is null)
            {
                return new Response<CigaretteDetails>()
                {
                    Succeeded = false,
                    Message = "Cigarette Detail not found with specified id"
                };
            }
            return null;
        }
    }
}

