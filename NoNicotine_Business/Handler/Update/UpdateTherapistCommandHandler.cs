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
    public class UpdateTherapistCommandHandler : IRequestHandler<UpdateTherapistCommand, Response<Therapist>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateTherapistCommandHandler> _logger;
        public UpdateTherapistCommandHandler(AppDbContext context, ILogger<UpdateTherapistCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<Therapist>> Handle(UpdateTherapistCommand request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var therapist = await _context.Therapist.Where(patient => patient.IdentityUserId == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                return new Response<Therapist>()
                {
                    Succeeded = false,
                    Message = "Therapist not found with specified id"
                };
            }

            therapist.BirthDate = request.BirthDate ?? therapist.BirthDate;
            therapist.Sex = request.Sex ?? therapist.Sex;
            therapist.Name = request.Name ?? therapist.Name;

            _context.Therapist.Update(therapist);

            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<Therapist>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation("Therapist with ID {therapistId} updated", therapist.ID);

            return new Response<Therapist>()
            {
                Succeeded = true,
                Data = therapist
            };
        }

        private static Response<Therapist>? ValidateRequest(UpdateTherapistCommand request)
        {
            if (request == null || request.Id == string.Empty)
            {
                return new Response<Therapist>()
                {
                    Succeeded = false,
                    Message = "You must specify a user Id to update"
                };
            }

            if (request.Name != null && request.Name == string.Empty)
            {
                return new Response<Therapist>()
                {
                    Succeeded = false,
                    Message = "You can't update a therapist with an empty name"
                };
            }

            if (request.Sex != null && request.Sex != 'M' && request.Sex != 'F')
            {
                return new Response<Therapist>()
                {
                    Succeeded = false,
                    Message = "You must specify a sex (M or F)"
                };
            }

            return null;
        }


    }
}
