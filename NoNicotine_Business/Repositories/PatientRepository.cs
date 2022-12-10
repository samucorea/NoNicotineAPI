using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
  public class PatientRepository : IPatientRepository
  {
    private readonly AppDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;
    public PatientRepository(AppDbContext context, UserManager<IdentityUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }
    public async Task<int> CreatePatientAsync(Patient patient, CancellationToken cancellationToken)
    {
      await _context.Patient.AddAsync(patient, cancellationToken);

      return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PatientDTO?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
      var patient = await _context.Patient
      .Include("Therapist")
      .Include("PatientConsumptionMethods")
      .Include("PatientConsumptionMethods.CigarDetails")
      .Include("PatientConsumptionMethods.CigaretteDetails")
      .Include("PatientConsumptionMethods.ElectronicCigaretteDetails")
      .Include("PatientConsumptionMethods.HookahDetails")
      .Where(patient => patient.IdentityUserId == userId)
      .FirstOrDefaultAsync(cancellationToken);
      if (patient == null)
      {
        return null;
      }
      Therapist? responseTherapist = null;
      if (patient.Therapist != null)
      {
        responseTherapist = new Therapist
        {
          Name = patient.Therapist.Name,
          Sex = patient.Therapist.Sex,
          BirthDate = patient.Therapist.BirthDate,
          Identification = patient.Therapist.Identification,
          IdentificationType = patient.Therapist.IdentificationType,
          Active = patient.Therapist.Active,
          IdentityUserId = patient.Therapist.IdentityUserId,
        };
      }

      var responsePatientConsumptionMethods = new PatientConsumptionMethodsDTO(){
        CigarDetails= patient.PatientConsumptionMethods?.CigarDetails,
        CigaretteDetails = patient.PatientConsumptionMethods?.CigaretteDetails,
        HookahDetails = patient.PatientConsumptionMethods?.HookahDetails,
        ElectronicCigaretteDetails = patient.PatientConsumptionMethods?.ElectronicCigaretteDetails,

      };

      var user = await _userManager.FindByIdAsync(patient.IdentityUserId);
      string email = "";
      if (user != null)
      {
        email = user.Email;
      }



      var responsePatient = new PatientDTO()
      {
        ID = patient.ID,
        Name = patient.Name,
        Sex = patient.Sex,
        BirthDate = patient.BirthDate,
        Identification = patient.Identification,
        IdentificationType = patient.IdentificationType,
        StartTime = patient.StartTime,
        Active = patient.Active,
        IdentityUserId = patient.IdentityUserId,
        TherapistId = patient.TherapistId,
        Therapist = responseTherapist,
        PatientConsumptionMethodsId = patient.PatientConsumptionMethodsId,
        PatientConsumptionMethods = responsePatientConsumptionMethods,
        Email = email
      };

      return responsePatient;
    }

    public async Task<TherapistPatient?> GetTherapistPatientAsync(string therapistId, string patientId, CancellationToken cancellationToken)
    {
      var patient = await _context.Patient.Where(patient => patient.ID == patientId && patient.TherapistId == therapistId).FirstOrDefaultAsync(cancellationToken);

      if (patient == null)
      {
        return null;
      }

      TherapistPatient therapistPatient = new()
      {
        Name = patient.Name,
        Sex = patient.Sex,
        BirthDate = patient.BirthDate,
        StartTime = patient.StartTime,
        Active = patient.Active,
        IdentityUserId = patient.IdentityUserId,
        TherapistId = patient.TherapistId,
        PatientConsumptionMethodsId = patient.PatientConsumptionMethodsId,
        PatientConsumptionMethods = patient.PatientConsumptionMethods,
        ID = patient.ID,
        CreatedAt = patient.CreatedAt
      };

      return therapistPatient;
    }

    public async Task<List<TherapistPatient>?> GetTherapistPatientsAsync(string therapistId, CancellationToken cancellationToken)
    {
      var patients = await _context.Patient.Where(patient => patient.TherapistId == therapistId).ToListAsync(cancellationToken);

      if (patients.Count < 1)
      {
        return null;
      }

      List<TherapistPatient> therapistPatients = new List<TherapistPatient>();

      foreach (var patient in patients)
      {
        TherapistPatient therapistPatient = new()
        {
          Name = patient.Name,
          Sex = patient.Sex,
          IdentityUserId = patient.IdentityUserId,
          BirthDate = patient.BirthDate,
          StartTime = patient.StartTime,
          Active = patient.Active,
          TherapistId = patient.TherapistId,
          PatientConsumptionMethodsId = patient.PatientConsumptionMethodsId,
          PatientConsumptionMethods = patient.PatientConsumptionMethods,
          ID = patient.ID,
          CreatedAt = patient.CreatedAt
        };
        therapistPatients.Add(therapistPatient);
      }

      return therapistPatients;
    }
  }
}
