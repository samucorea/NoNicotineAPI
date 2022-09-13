using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Commands
{
    public class CreatePatientCommand:IRequest<bool>
    {
        public string? Name { get; set; }
        public char Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
