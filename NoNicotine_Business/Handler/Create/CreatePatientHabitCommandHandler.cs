using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Business.Queries;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Create
{
    public class CreatePatientHabitCommandHandler : IRequestHandler<CreatePatientHabitCommand, Response<PatientHabit>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreatePatientHabitCommand> _logger;
        public CreatePatientHabitCommandHandler(AppDbContext context, ILogger<CreatePatientHabitCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<PatientHabit>> Handle(CreatePatientHabitCommand request, CancellationToken cancellationToken)
        {
            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == request.UserId).FirstOrDefaultAsync();
            if (patient == null)
            {
                return new Response<PatientHabit>()
                {
                    Succeeded = false,
                    Message = "Patient not found with patient id specified"
                };
            }

            var habit = await _context.Habit.Where(habit => string.Equals(habit.Name, request.Name)).FirstOrDefaultAsync();
            if (habit == null)
            {
                return new Response<PatientHabit>()
                {
                    Message = "Habit does not exist",
                    Succeeded = false
                };
            }

            var patientHabit = new PatientHabit()
            {
                HabitId = habit.ID,
                Monday = request.Monday,
                Tuesday = request.Tuesday,
                Wednesday = request.Wednesday,
                Thursday = request.Thursday,
                Friday = request.Friday,
                Saturday = request.Saturday,
                Sunday = request.Sunday,
                Hour = request.Hour,
                PatientId = patient.ID
            };

            await _context.PatientHabit.AddAsync(patientHabit);

            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
            {
                return new Response<PatientHabit>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            return new Response<PatientHabit>()
            {
                Data = patientHabit,
                Succeeded = true
            };
        }

        private static Response<PatientHabit>? ValidateRequest(CreatePatientHabitCommand request)
        {
            if (request.Name == string.Empty)
            {
                return new Response<PatientHabit>()
                {
                    Succeeded = false,
                    Message = "You must specify a name for the habit"
                };
            }

            if (!CheckAnyDaysSelected(request))
            {
                return new Response<PatientHabit>()
                {
                    Succeeded = false,
                    Message = "You must specify at least a day for the habit"
                };
            }

            return null;
        }

        private static bool CheckAnyDaysSelected(CreatePatientHabitCommand request)
        {
            return request.Monday || request.Tuesday || request.Wednesday ||
                request.Thursday || request.Friday || request.Saturday || request.Sunday;
        }
    }
}
