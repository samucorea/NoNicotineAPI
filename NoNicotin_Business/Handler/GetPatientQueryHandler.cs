using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoNicotin_Business.Commands;
using NoNicotin_Business.Queries;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoNicotin_Business.Handler
{
    public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, Response<Patient>>
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<CreatePatientCommandHandler> _logger;
        private const string PATIENT_ROLE = "patient";
        public GetPatientQueryHandler(AppDbContext context, UserManager<IdentityUser> userManager, ILogger<CreatePatientCommandHandler> logger, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<Response<Patient>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _context.Patient.FindAsync(request.Id, cancellationToken);
            if (patient == null)
            {
                return new Response<Patient>
                {
                    Succeeded = false,
                    Message = "Could not find Patient with specified id"
                };
            }

            return new Response<Patient>
            {
                Succeeded = true,
                Data = patient
            };

        }

        private static Response<Patient>? ValidateRequest(GetPatientQuery request)
        {
            if (request.Id == string.Empty)
            {
                return new Response<Patient>
                {
                    Succeeded = false,
                    Message = "Missing Patient Id"
                };
            }

            return null;
        }

    }
}
