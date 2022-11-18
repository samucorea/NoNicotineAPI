using MediatR;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Update
{
    public class UpdateAcceptDenyLinkrequestCommand : IRequest<Response<bool>>
    {
        public string LinkrequestID { get; set; }
        public bool Approval { get; set; }
    }
}
