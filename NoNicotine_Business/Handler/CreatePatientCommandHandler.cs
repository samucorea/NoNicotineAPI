using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Repositories;
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
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Response<Patient>>
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreatePatientCommandHandler> _logger;
        private readonly IPatientRepository _patientRepository;
        private readonly AppDbContext _context;
        private const string PATIENT_ROLE = "patient";
        public CreatePatientCommandHandler( UserManager<IdentityUser> userManager,
            ILogger<CreatePatientCommandHandler> logger, IPatientRepository patientRepository, AppDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _patientRepository = patientRepository;
            _context = context;
        }

        public async Task<Response<Patient>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                //checks if email is already registred
                var response = ValidateRequest(request);
                if (response != null)
                {
                    return response;
                }

                var identityUser = new IdentityUser { UserName = request.Email, Email = request.Email };
                var resultIdentity = await _userManager.CreateAsync(identityUser, request.Password);
 
                if (!resultIdentity.Succeeded)
                {
                    return new Response<Patient>
                    {
                        Succeeded = false,
                        Message = $"Could not create user: {resultIdentity.Errors.First().Description}"
                    };
                }

                var patient = new Patient()
                {
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    Sex = request.Sex,
                    IdentityUserId = identityUser.Id,
                    Identification = request.Identification,
                    IdentificationType = request.IdentificationPatientType,
                    StartTime= DateTime.Now,
                };

                resultIdentity = await _userManager.AddToRoleAsync(identityUser, PATIENT_ROLE);
                if (!resultIdentity.Succeeded)
                {
                    return new Response<Patient>()
                    {
                        Succeeded = false,
                        Message = "Could not assign patient role to user",
                    };
                }

                var result = await _patientRepository.CreatePatientAsync(patient, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Saving changes when creating patient");

                    return new Response<Patient>
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }

                var patientConsumptionMethods = await _patientRepository.CreateEmptyPatientConsumptionMethods(patient.ID, cancellationToken);
                if(patientConsumptionMethods == null)
                {
                    transaction.Rollback();

                    return new Response<Patient>()
                    {
                        Succeeded = false,
                        Message = "Could not create empty patient consumption methods"
                    };
                }

                transaction.Commit();

                patient.PatientConsumptionMethodsId = patientConsumptionMethods.ID;

                return new Response<Patient>
                {
                    Succeeded = true,
                    Data = patient
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating patient: {errMessage}", ex.Message);
                transaction.Rollback();
                return new Response<Patient>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }



        }

        private Response<Patient>? ValidateRequest(CreatePatientCommand request)
        {

            if (request.Email == string.Empty)
            {
                return new Response<Patient>
                {
                    Message = "You must specify a valid email",
                    Succeeded = false
                };
            }

            if (request.Password == string.Empty)
            {
                return new Response<Patient>
                {
                    Message = "You must specify a password",
                    Succeeded = false
                };
            }

            if (_userManager.FindByEmailAsync(request.Email).Result is not null)
            {
                return new Response<Patient>
                {
                    Message = "Email already taken",
                    Succeeded = false
                };
            }

            if (request.Name == string.Empty)
            {
                return new Response<Patient>
                {
                    Message = "You must specify a patient name",
                    Succeeded = false
                };
            }

            if (request.Sex == ' ')
            {
                return new Response<Patient>
                {
                    Message = "You must specify the patient sex",
                    Succeeded = false
                };
            }

            if (request.BirthDate.AddYears(18) > DateTime.Now)
            {

                return new Response<Patient>
                {
                    Message = "You must be 18 years old or greater to register",
                    Succeeded = false
                };
            }

            if (request.Identification == string.Empty)
            {
                return new Response<Patient>
                {
                    Message = "You must specify the patient identification number",
                    Succeeded = false
                };
            }

            if (request.IdentificationPatientType == string.Empty)
            {
                return new Response<Patient>
                {
                    Message = "You must specify the identification type",
                    Succeeded = false
                };
            }


            return null;
        }

    }
}
