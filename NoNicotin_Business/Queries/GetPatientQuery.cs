using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Queries
{
    public class GetPatientQuery : IRequest<Response<Patient>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
