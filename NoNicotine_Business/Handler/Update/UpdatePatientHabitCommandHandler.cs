using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoNicotine_Business.Commands.Update;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Handler.Update
{
    public class UpdatePatientHabitCommandHandler : IRequestHandler<UpdatePatientHabitCommand, Response<PatientHabit>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdatePatientHabitCommandHandler> _logger;
        public UpdatePatientHabitCommandHandler(AppDbContext context, ILogger<UpdatePatientHabitCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<PatientHabit>> Handle(UpdatePatientHabitCommand request, CancellationToken cancellationToken)
        {


            var patient = await _context.Patient.Where(patient => patient.IdentityUserId == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (patient == null)
            {
                return new Response<PatientHabit>()
                {
                    Succeeded = false,
                    Message = "Patient not found with specified id"
                };
            }

            var patientHabit = await _context.PatientHabit.Where(patientHabit => patientHabit.ID == request.PatientHabitId).FirstOrDefaultAsync(cancellationToken);
            if (patientHabit == null)
            {
                return new Response<PatientHabit>
                {
                    Message = "Habit not found for patient",
                    Succeeded = false
                };
            }

            patientHabit.Hour = request.Hour ?? patientHabit.Hour;
            patientHabit.Monday = request.Monday ?? patientHabit.Monday;
            patientHabit.Tuesday = request.Tuesday ?? patientHabit.Tuesday;
            patientHabit.Wednesday = request.Wednesday ?? patientHabit.Wednesday;
            patientHabit.Thursday = request.Thursday ?? patientHabit.Thursday;
            patientHabit.Friday = request.Friday ?? patientHabit.Friday;
            patientHabit.Saturday = request.Saturday ?? patientHabit.Saturday;
            patientHabit.Sunday = request.Sunday ?? patientHabit.Sunday;


            _context.PatientHabit.Update(patientHabit);

            var result = await _context.SaveChangesAsync();

            if (result < 1)
            {
                return new Response<PatientHabit>()
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }

            _logger.LogInformation("Patient habit with ID {patientHabitId} updated", patientHabit.ID);

            return new Response<PatientHabit>()
            {
                Succeeded = true,
                Data = patientHabit
            };
        }


    }
}
