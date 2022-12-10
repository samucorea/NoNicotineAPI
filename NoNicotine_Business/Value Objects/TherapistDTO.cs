using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NoNicotine_Data.Entities;

namespace NoNicotineAPI.Models
{
  public class TherapistDTO : Therapist
  {
    public string Email { get; set; } = string.Empty;
  }
}
