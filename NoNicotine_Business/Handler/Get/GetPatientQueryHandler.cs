using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
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
  public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, Response<PatientDTO>>
  {

    private readonly IPatientRepository _patientRepository;
    public GetPatientQueryHandler(IPatientRepository patientRepository)
    {
      _patientRepository = patientRepository;
    }

    public async Task<Response<PatientDTO>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {

      var response = ValidateRequest(request);
      if (response != null)
      {
        return response;
      }

      var patient = await _patientRepository.GetPatientByUserIdAsync(request.UserId, cancellationToken);
      if (patient == null)
      {
        return new Response<PatientDTO>
        {
          Succeeded = false,
          Message = "Could not find Patient with specified id"
        };
      }



      return new Response<PatientDTO>
      {
        Succeeded = true,
        Data = patient
      };

    }

    private static Response<PatientDTO>? ValidateRequest(GetPatientQuery request)
    {
      if (request.UserId == string.Empty)
      {
        return new Response<PatientDTO>
        {
          Succeeded = false,
          Message = "Missing Patient Id"
        };
      }

      return null;
    }

  }
}
