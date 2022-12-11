using MediatR;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Delete
{
    public class DeleteCigarreteDetailsCommand : IRequest<Response<bool>>
    {
        public string PatientConsumptionId { get; set; }
    }
}
