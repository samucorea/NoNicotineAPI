using MediatR;
using Microsoft.EntityFrameworkCore;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Get
{
    internal class GetAllPatientHabitsQueryHandler : IRequestHandler<GetAllPatientHabitsQuery, Response<List<PatientHabit>>>
    {
        private readonly AppDbContext _context;
        public GetAllPatientHabitsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<PatientHabit>>> Handle(GetAllPatientHabitsQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == request.UserId).FirstOrDefaultAsync();
            if (patient == null)
            {
                return new Response<List<PatientHabit>>
                {
                    Succeeded = false,
                    Message = "Could not find Patient with specified id"
                };
            }

            var patientHabits = await _context.PatientHabit.Where(patientHabit => patientHabit.PatientId == patient.ID).Include("Habit").ToListAsync();

            return new Response<List<PatientHabit>>
            {
                Succeeded = true,
                Data = patientHabits
            };

        }

        private static Response<List<PatientHabit>>? ValidateRequest(GetAllPatientHabitsQuery request)
        {
            if (request.UserId == string.Empty)
            {
                return new Response<List<PatientHabit>>
                {
                    Succeeded = false,
                    Message = "Missing user ID"
                };
            }

            return null;
        }
    }
}
