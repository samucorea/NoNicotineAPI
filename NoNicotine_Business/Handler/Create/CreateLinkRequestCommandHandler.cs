using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Create
{
    public class CreateLinkRequestCommandHandler : IRequestHandler<CreateLinkRequestCommand, Response<LinkRequest>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        public CreateLinkRequestCommandHandler(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public async Task<Response<LinkRequest>> Handle(CreateLinkRequestCommand request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var therapist = await _context.Therapist.Where(therapist => therapist.IdentityUserId == request.TherapistUserId).FirstOrDefaultAsync(cancellationToken);
            if (therapist == null)
            {
                return new Response<LinkRequest>
                {
                    Message = "Therapist not found",
                    Succeeded = false
                };
            }

            var patientUser = await _userManager.FindByEmailAsync(request.PatientEmail);
            if(patientUser == null)
            {
                return new Response<LinkRequest>
                {
                    Message = "Patient not found with specified email",
                    Succeeded = false
                };
            }

            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == patientUser.Id).FirstOrDefaultAsync(cancellationToken);
            if (patient == null)
            {
                return new Response<LinkRequest>
                {
                    Message = "Patient not found with specified email",
                    Succeeded = false
                };
            }

            var linkRequest = new LinkRequest
            {
                PatientId = patient.ID,
                TherapistId = therapist.ID,
            };

            await _context.LinkRequest.AddAsync(linkRequest, cancellationToken);

            var result = await _context.SaveChangesAsync(cancellationToken);
            if(result < 1)
            {
                return new Response<LinkRequest>
                {
                    Message = "Something went wrong",
                    Succeeded = false
                };
            }

            return new Response<LinkRequest>
            {
                Data = linkRequest,
                Succeeded = true
            };
        }

        

        private static Response<LinkRequest>? ValidateRequest(CreateLinkRequestCommand request)
        {
            if (request.TherapistUserId == string.Empty)
            {
                return new Response<LinkRequest>
                {
                    Message = "Missing therapist user id",
                    Succeeded = false
                };
            }

            if (request.PatientEmail == string.Empty)
            {
                return new Response<LinkRequest>
                {
                    Message = "Missing patient email",
                    Succeeded = false
                };
            }

            return null;
        }
    }
}
