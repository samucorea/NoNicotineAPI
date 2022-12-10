using NoNicotineAPI.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Delete
{
    public class DeleteCigarDetailsCommand : IRequest<Response<bool>>
    {
        public string PatientConsumptionId { get; set; }
    }
}
