using NoNicotineAPI.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Delete;
using NoNicotine_Business.Commands.Update;
using NoNicotine_Business.Handler.Update;
using NoNicotine_Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Delete
{
    public class DeleteCigarDetailsCommandHandler : IRequestHandler<DeleteCigarDetailsCommand,Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteCigarDetailsCommandHandler> _logger;
        public DeleteCigarDetailsCommandHandler(AppDbContext context, ILogger<DeleteCigarDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteCigarDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isPatientConsumptionMethod = _context.PatientConsumptionMethods.Where(x => x.ID == request.PatientConsumptionId).FirstOrDefault();
                if (isPatientConsumptionMethod is null || string.IsNullOrEmpty(isPatientConsumptionMethod.CigarDetailsId))
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "You must specify a valid id to update  || No cigar detail related",
                        Data = false
                    };
                }

                // find and remove cigar detail
                var isCigarDetail = _context.CigarDetails.Where(x => x.ID == isPatientConsumptionMethod.CigarDetailsId).FirstOrDefault();
                if (isCigarDetail is not null)
                    _context.CigarDetails.Remove(isCigarDetail);
                // removes cigar detail from patient consumption method
                isPatientConsumptionMethod.CigarDetailsId = null;
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
                    Message = "Cigar detail removed",
                    Data = true
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting cigar detail : {ex.Message}");
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
