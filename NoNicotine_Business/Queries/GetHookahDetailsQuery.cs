using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Queries
{
    public class GetHookahDetailsQuery : IRequest<Response<HookahDetails>>
    {
        public string PatientConsumtionId { get; set; }
    }
}
