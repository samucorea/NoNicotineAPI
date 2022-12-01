using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Delete;
using NoNicotine_Data.Context;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Delete
{
    public class DeleteElectronicCigarreteDetailsCommandHandler : IRequestHandler<DeleteElectronicCigarreteDetailsCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteElectronicCigarreteDetailsCommandHandler> _logger;
        public DeleteElectronicCigarreteDetailsCommandHandler(AppDbContext context, ILogger<DeleteElectronicCigarreteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(DeleteElectronicCigarreteDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isPatientConsumptionMethod = _context.PatientConsumptionMethods.Where(x => x.ID == request.PatientConsumptionId).FirstOrDefault();
                if (isPatientConsumptionMethod is null || string.IsNullOrEmpty(isPatientConsumptionMethod.ElectronicCigaretteDetailsId))
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "You must specify a valid id to update  || No electronic cigarrete detail related",
                        Data = false
                    };
                }

                // find and remove cigar detail
                var isECigarretteDetail = _context.ElectronicCigaretteDetails.Where(x => x.ID == isPatientConsumptionMethod.ElectronicCigaretteDetailsId).FirstOrDefault();
                if (isECigarretteDetail is not null)
                    _context.ElectronicCigaretteDetails.Remove(isECigarretteDetail);
                // removes electronic cigarrete from patient consumption method
                isPatientConsumptionMethod.ElectronicCigaretteDetailsId = null;
                _context.PatientConsumptionMethods.Update(isPatientConsumptionMethod);
                var result = await _context.SaveChangesAsync();
                if (result < 1)
                {
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "Something went wrong",
                        Data = false
                    };
                }

                return new Response<bool>
                {
                    Succeeded = true,
                    Message = "Electronic cigarrete detail removed",
                    Data = true
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting electronic cigar detail : {ex.Message}");
                return new Response<bool>
                {
                    Succeeded = false,
                    Message = "Something went wrong",
                    Data = false
                };
            }
        }
    }
}