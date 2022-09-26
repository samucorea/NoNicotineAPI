using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoNicotin_Business.Commands;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoNicotin_Business.Handler
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand,bool>
    {
 
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreatePatientCommandHandler> _logger;
        public CreatePatientCommandHandler(AppDbContext context, UserManager<IdentityUser> userManager, ILogger<CreatePatientCommandHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //checks if email is already registred
                if (_userManager.FindByEmailAsync(request.Email).Result is not null)
                {
                    return false;
                }

                if(request.Name == null)
                {
                    return false;
                }
                //Creates IdentityUser
                var identityUser = new IdentityUser {UserName = request.Email, Email = request.Email };
                var resultIdentity = await _userManager.CreateAsync(identityUser, request.Password);

                if (!resultIdentity.Succeeded)
                {
                    return false;
                }
                //
                var tempIdentityUser = _userManager.FindByEmailAsync(request.Email);
                var patient = new Patient()
                {
                    Name = request.Name,
                    BirthDate = request.BirthDate,
                    Sex = request.Sex,
                    IdentityUserId = tempIdentityUser.Id,
                    Identification = request.Identification,
                    IdentificationType = request.IdentificationPatientType
                };
                // adds patient to db
                _context.Patients.Add(patient);

                var result = _context.SaveChanges();

                return result > 0;
            }
            catch (Exception ex)
            {

                var innerException = "No inner exception was detected";
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                    
                }

                _logger.LogError(innerException);

                throw;
            }
        }
    }
}
