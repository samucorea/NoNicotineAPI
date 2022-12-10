using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NoNicotine_Data.Entities;

namespace NoNicotineAPI.Models
{
  public class PatientDTO
  {
    public string ID {get; set;}
    public string Name { get; set; }
    public char Sex { get; set; }
    public DateTime BirthDate { get; set; }
    public string Identification { get; set; } = string.Empty;
    public string IdentificationType { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public bool Active { get; set; }
    public string IdentityUserId { get; set; } = string.Empty;
    public string? TherapistId { get; set; }
    public Therapist? Therapist { get; set; }
    public string PatientConsumptionMethodsId { get; set; } = string.Empty;
    public PatientConsumptionMethodsDTO? PatientConsumptionMethods { get; set; }
    public string Email { get; set; } = string.Empty;


  }
}
