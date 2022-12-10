using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Queries;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoNicotine_Business.Handler.Get
{
  public class GetTherapistQueryHandler : IRequestHandler<GetTherapistQuery, Response<TherapistDTO>>
  {

    private readonly AppDbContext _context;
    private readonly ILogger<GetTherapistQueryHandler> _logger;

    private readonly UserManager<IdentityUser> _userManager;
    public GetTherapistQueryHandler(AppDbContext context, ILogger<GetTherapistQueryHandler> logger, UserManager<IdentityUser> userManager)
    {
      _context = context;
      _logger = logger;
      _userManager = userManager;
    }

    public async Task<Response<TherapistDTO>> Handle(GetTherapistQuery request, CancellationToken cancellationToken)
    {

      var therapist = await _context.Therapist.Where(therapist => therapist.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
      if (therapist == null)
      {
        return new Response<TherapistDTO>
        {
          Succeeded = false,
          Message = "Could not find Therapist with specified id"
        };
      }

      var user = await _userManager.FindByIdAsync(therapist.IdentityUserId);
      string email = "";
      if (user != null)
      {
        email = user.Email;
      }


      var responseTherapist = new TherapistDTO()
      {
        Name = therapist.Name,
        Sex = therapist.Sex,
        BirthDate = therapist.BirthDate,
        Identification = therapist.Identification,
        IdentificationType = therapist.IdentificationType,
        Active = therapist.Active,
        IdentityUserId = therapist.IdentityUserId,
        Email = email
      };




      return new Response<TherapistDTO>
      {
        Succeeded = true,
        Data = responseTherapist
      };

    }
  }
}
