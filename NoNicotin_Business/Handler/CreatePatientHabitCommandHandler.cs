using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NoNicotin_Business.Commands;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Handler
{
    public class CreatePatientHabitCommandHandler : IRequestHandler<CreatePatientHabitCommand, Response<PatientHabit>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<CreatePatientHabitCommandHandler> _logger;
        public CreatePatientHabitCommandHandler(AppDbContext context, ILogger<CreatePatientHabitCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<PatientHabit>> Handle(CreatePatientHabitCommand request, CancellationToken cancellationToken)
        {
			try
			{
                // validate FK exists 
                var isPatient = await _context.Patient.FindAsync(request.PatientId);
                var isHabit = await _context.Habit.FindAsync(request.HabitId);
                var isHabitSchedule = await _context.HabitSchedule.FindAsync(request.HabitScheduleId);
                if (isPatient is null)
                {
                    return new Response<PatientHabit>
                    {
                        Succeeded = false,
                        Message = "Patient not found"
                    };
                }
                if (isHabit is null)
                {
                    return new Response<PatientHabit>
                    {
                        Succeeded = false,
                        Message = "Habit not found"
                    };
                }
                if (isHabitSchedule is null)
                {
                    return new Response<PatientHabit>
                    {
                        Succeeded = false,
                        Message = "Habit schedule not found"
                    };
                }

                var newPatientHabit = new PatientHabit()
                {
                    PatientId = request.PatientId,
                    HabitId = request.HabitId,
                    HabitScheduleId = request.HabitScheduleId,
                };

                _context.PatientHabit.Add(newPatientHabit);
                var result = _context.SaveChanges();
                if(result > 0)
                {
                    return new Response<PatientHabit>
                    {
                        Succeeded = true,
                        Data = newPatientHabit
                    };
                }
                else
                {
                    _logger.LogError("Saving changes when creating patient");
                    return new Response<PatientHabit>
                    {
                        Succeeded = false,
                        Message = "Something went wrong"
                    };
                }
            }
			catch (Exception ex) 
			{
                _logger.LogError(ex.ToString());
                return new Response<PatientHabit>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
