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
    public class DeleteHookahDetailsCommandHandler : IRequestHandler<DeleteHookahDetailsCommand, Response<bool>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<DeleteHookahDetailsCommandHandler> _logger;
        public DeleteHookahDetailsCommandHandler(AppDbContext context, ILogger<DeleteHookahDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(DeleteHookahDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isPatientConsumptionMethod = _context.PatientConsumptionMethods.Where(x => x.ID == request.PatientConsumptionId).FirstOrDefault();
                if (isPatientConsumptionMethod is null || string.IsNullOrEmpty(isPatientConsumptionMethod.HookahDetailsId))
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "You must specify a valid id to update  || No hookah details related",
                        Data = false
                    };
                }

                // find and remove hooka detail
                var isHookahDetail = _context.HookahDetails.Where(x => x.ID == isPatientConsumptionMethod.HookahDetailsId).FirstOrDefault();
                if (isHookahDetail is not null)
                    _context.HookahDetails.Remove(isHookahDetail);
                // removes hookah detail from patient consumption method
                isPatientConsumptionMethod.HookahDetailsId = null;
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
                    Message = "Hookah detail removed",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting hookah cigar detail : {ex.Message}");
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

