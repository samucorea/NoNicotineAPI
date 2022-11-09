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
    public class UpdateElectronicCigaretteDetailsCommand : IRequest<Response<ElectronicCigaretteDetails>>
    {
        public short? cartridgeLifespan { get; set; }
        public short? unitsPerBox { get; set; }
        public decimal? boxPrice { get; set; }
        public string patientConsumptionId { get; set; }
    }
}
