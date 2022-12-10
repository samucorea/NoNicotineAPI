using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using NoNicotine_Business.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoNicotine_Business.Services;

namespace NoNicotine_Business.Handler.Create
{
    public class CreateTherapistCommandHandler : IRequestHandler<CreateTherapistCommand, Response<CreateTherapistResponse>>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreateTherapistCommandHandler> _logger;
        private const string THERAPIST_ROLE = "therapist";

        private readonly IAuthenticationService _authenticationService;
        public CreateTherapistCommandHandler(AppDbContext context, UserManager<IdentityUser> userManager, ILogger<CreateTherapistCommandHandler> logger, IAuthenticationService authenticationService)
        {
            _context = context;
            _userManager = userManager;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        public async Task<Response<CreateTherapistResponse>> Handle(CreateTherapistCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //Checks if email is already registred
                var response = ValidateRequest(request);
                if (response != null)
                {
                    return response;
                }

                var identityUser = new IdentityUser { UserName = request.Email, Email = request.Email };
                var resultIdentity = await _userManager.CreateAsync(identityUser, request.Password);
                if (!resultIdentity.Succeeded)
                {
                    return new Response<CreateTherapistResponse>
                    {
                        Succeeded = false,
                        Message = "Could not create user"
                    };
                }

                var tempIdentityUser = await _userManager.FindByEmailAsync(request.Email);
                if (tempIdentityUser == null)
                {
                    return new Response<CreateTherapistResponse>
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }
                var therapist = new Therapist()
                {
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    Sex = request.Sex,
                    IdentityUserId = tempIdentityUser.Id,
                    Identification = request.Identification,
                    IdentificationType = request.IdentificationType
                };

                resultIdentity = await _userManager.AddToRoleAsync(tempIdentityUser, THERAPIST_ROLE);
                if (!resultIdentity.Succeeded)
                {
                    return new Response<CreateTherapistResponse>()
                    {
                        Succeeded = false,
                        Message = "Could not assign therapist role to user",
                    };
                }

                _context.Therapist.Add(therapist);

                var result = _context.SaveChanges();
                if (result <= 0)
                {
                    _logger.LogError("Saving changes when creating therapist");

                    return new Response<CreateTherapistResponse>
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                transaction.Commit();

                return new Response<CreateTherapistResponse>
                {
                    Succeeded = true,
                    Data = new CreateTherapistResponse {
                        Therapist = therapist,
                        ConfirmationToken = await _authenticationService.GenerateEmailConfirmationUrlTokenAsync(identityUser),
                        Email =identityUser.Email
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating therapist: {errMessage}", ex.Message);
                transaction.Rollback();
                return new Response<CreateTherapistResponse>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

        }

        private Response<CreateTherapistResponse>? ValidateRequest(CreateTherapistCommand request)
        {
            if (request.Email == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify a valid email",
                    Succeeded = false
                };
            }

            if (request.Password == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify a password",
                    Succeeded = false
                };
            }

            if (_userManager.FindByEmailAsync(request.Email).Result is not null)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "Email already taken",
                    Succeeded = false
                };
            }

            if (request.Name == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify a therapist name",
                    Succeeded = false
                };
            }

            if (request.Sex == ' ')
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify the therapist sex",
                    Succeeded = false
                };
            }

            if (request.BirthDate.AddYears(18) > DateTime.Now)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must be 18 years old or greater to register",
                    Succeeded = false
                };
            }

            if (request.Identification == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify the therapist identification number",
                    Succeeded = false
                };
            }

            if (request.IdentificationType == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify the identification type",
                    Succeeded = false
                };
            }

            if (request.Email == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify a valid email",
                    Succeeded = false
                };
            }

            if (request.Password == string.Empty)
            {
                return new Response<CreateTherapistResponse>
                {
                    Message = "You must specify a password",
                    Succeeded = false
                };
            }

            return null;
        }
    }
}
