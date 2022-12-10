using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NoNicotine_Data.Entities;

namespace NoNicotineAPI.Models
{
  public class PatientConsumptionMethodsDTO
  {
    public CigaretteDetails? CigaretteDetails { get; set; }
    public ElectronicCigaretteDetails? ElectronicCigaretteDetails { get; set; }
    public CigarDetails? CigarDetails { get; set; }

    public HookahDetails? HookahDetails { get; set; }
  }
}
