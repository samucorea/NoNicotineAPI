using MediatR;
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
        public CreatePatientCommandHandler(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                //Creates IdentityUser
                var identityUser = new IdentityUser { UserName = request.Name, Email = request.Email };
                var resultIdentity = await _userManager.CreateAsync(identityUser, request.Password);
                //
                if (resultIdentity.Succeeded)
                {
                    var tempIdentityUser = _userManager.FindByEmailAsync(request.Email);
                    var patient = new Patient()
                    {
                        Name = request.Name,
                        BirthDate = request.BirthDate,
                        Sex = request.Sex,
                        IdentityUserId = tempIdentityUser.Result.Id
                    };
                    // adds patient to db
                    _context.Patients.Add(patient);
                    var result = _context.SaveChanges();
                    if (result > 0)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Agregar logger
                var innerException = "'No se ha detectado ninguna inner excepption'";
                if (ex.InnerException != null)
                    innerException = ex.InnerException.Message;
                throw;
            }
        }
    }
}
