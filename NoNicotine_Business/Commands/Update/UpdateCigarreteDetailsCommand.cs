using MediatR;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Handler;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Update
{
    public class UpdateCigarreteDetailsCommand : IRequest<Response<CigaretteDetails>>
    {
        public string PatientMethodId { get; set; }
        public short? unitsPerDay { get; set; }
        public short? daysPerWeek { get; set; }
        public short? unitsPerBox { get; set; }
        public decimal? boxPrice { get; set; }
    }
}
