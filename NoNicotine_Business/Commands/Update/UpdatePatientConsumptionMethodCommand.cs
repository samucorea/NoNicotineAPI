using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Update
{
    public class UpdatePatientConsumptionMethodCommand : IRequest<Response<PatientConsumptionMethods>>
    {
        public string PatientMethodId { get; set; }
        public string? CigaretteDetailsId { get; set; }
        public string? ElectronicCigaretteDetailsId { get; set; }
        public string? CigarDetailsId { get; set; }
        public string? HookahDetailsId { get; set; }
    }
}
