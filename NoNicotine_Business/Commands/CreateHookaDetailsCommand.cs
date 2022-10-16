using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands
{
    public class CreateHookaDetailsCommand : IRequest<Response<HookahDetails>>
    {
        public short daysPerWeek { get; set; }
        public decimal setupPrice { get; set; }
        public string PatientConsumptionMethodsId { get; set; }
    }
}
