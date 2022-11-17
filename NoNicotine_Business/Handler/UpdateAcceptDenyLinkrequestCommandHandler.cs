using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
using NoNicotine_Data.Context;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler
{
    public class UpdateAcceptDenyLinkrequestCommandHandler : IRequestHandler<UpdateAcceptDenyLinkrequestCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateAcceptDenyLinkrequestCommandHandler> _logger;
        public UpdateAcceptDenyLinkrequestCommandHandler(AppDbContext context, ILogger<UpdateAcceptDenyLinkrequestCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<bool>> Handle(UpdateAcceptDenyLinkrequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = ValidateRequest(request);
                if (response != null)
                {
                    return response;
                }

                var isLinkRequest = await _context.LinkRequest.Where(x => x.ID == request.LinkRequestId && x.Patient.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
                if (isLinkRequest == null)
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "Link request not found with specified id",
                        Data = false
                    };
                }

                isLinkRequest.RequestAccepted = request.Approval;

                if (request.Approval)
                {
                     isLinkRequest.Patient.TherapistId = isLinkRequest.TherapistId;
                    _context.Patient.Update(isLinkRequest.Patient);
                }

                _context.LinkRequest.Update(isLinkRequest);

                var result = await _context.SaveChangesAsync();

                if (result < 1)
                {
                    return new Response<bool>()
                    {
                        Succeeded = false,
                        Message = "Something went wrong",
                        Data = false
                    };
                }

                _logger.LogInformation($"Link request with ID {request.LinkRequestId} updated");

                return new Response<bool>()
                {
                    Succeeded = true,
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

        private static Response<bool>? ValidateRequest(UpdateAcceptDenyLinkrequestCommand request)
        {
            if (request == null || request.LinkRequestId == string.Empty || request.UserId == string.Empty)
            {
                return new Response<bool>()
                {
                    Succeeded = false,
                    Message = "You must specify a valid id to update",
                    Data = false
                };
            }
            return null;
        }
    }
}
