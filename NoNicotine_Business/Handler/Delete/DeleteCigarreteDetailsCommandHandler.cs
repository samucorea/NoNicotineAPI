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
    public class DeleteCigarreteDetailsCommandHandler : IRequestHandler<DeleteCigarreteDetailsCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteCigarreteDetailsCommandHandler> _logger;
        public DeleteCigarreteDetailsCommandHandler(AppDbContext context, ILogger<DeleteCigarreteDetailsCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(DeleteCigarreteDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isPatientConsumptionMethod = _context.PatientConsumptionMethods.Where(x => x.ID == request.PatientConsumptionId).FirstOrDefault();
                if (isPatientConsumptionMethod is null || string.IsNullOrEmpty(isPatientConsumptionMethod.CigaretteDetailsId))
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "You must specify a valid id to update  || No cigarrete detail related",
                        Data = false
                    };
                }

                // find and remove cigarrete detail
                var isCigarreteDetail = _context.CigaretteDetails.Where(x => x.ID == isPatientConsumptionMethod.CigaretteDetailsId).FirstOrDefault();
                if (isCigarreteDetail is not null)
                    _context.CigaretteDetails.Remove(isCigarreteDetail);
                // removes cigarrete detail from patient consumption method
                isPatientConsumptionMethod.CigaretteDetailsId = null;
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
                    Message = "Cigarrete detail removed",
                    Data = true
                };

            }
            catch (Exception)
            {
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

