using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
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
    public class CreateCigarDetailsCommandHandler : IRequestHandler<CreateCigarDetailsCommand, Response<CigarDetails>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateCigarDetailsCommandHandler> _logger;
        public CreateCigarDetailsCommandHandler(AppDbContext context, ILogger<CreateCigarDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<CigarDetails>> Handle(CreateCigarDetailsCommand request, CancellationToken cancellationToken)
        {
			try
			{
                // validation 
                var isValidation = await ValidationRequest(request);
                if (isValidation is not null)
                {
                    return isValidation;
                }

                var isCigarDetails = new CigarDetails()
                {
                    unitsPerBox = request.unitsPerBox,
                    unitsPerDay = request.unitsPerDay,
                    boxPrice = request.boxPrice,
                    daysPerWeek = request.daysPerWeek,
                    PatientConsumptionMethodsId = request.PatientConsumptionMethodsId
                };

                await _context.CigarDetails.AddAsync(isCigarDetails);
                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<CigarDetails>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                return new Response<CigarDetails>()
                {
                    Succeeded = true,
                    Data = isCigarDetails
                };

            }
			catch (Exception ex)
			{
                _logger.LogError($"Error when creating a cigar detail : {ex.Message}");
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }

        private async Task<Response<CigarDetails>> ValidationRequest(CreateCigarDetailsCommand request)
        {
			try
			{
                // check if patient consumption method ID exists
                var isPatientConsumption = await _context.PatientConsumptionMethods.FindAsync(request.PatientConsumptionMethodsId);
                if (isPatientConsumption is null)
                {
                    return new Response<CigarDetails>()
                    {
                        Succeeded = false,
                        Message = "Invalid patient consumption method Id"
                    };
                }
			}
			catch (Exception ex)
			{
                _logger.LogError($"Error when validating a cigar detail request : {ex.Message}");
                return new Response<CigarDetails>()
                {
                    Succeeded = false,
                    Message = "Somenthing went wrong"
                };
            }
            return null;
        }
    }
}
