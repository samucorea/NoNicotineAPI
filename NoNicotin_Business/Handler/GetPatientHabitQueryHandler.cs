using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NoNicotin_Business.Queries;
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
    public class GetPatientHabitQueryHandler : IRequestHandler<GetPatientHabitQuery, Response<PatientHabit>>
    {
        private readonly AppDbContext _context;
       
        private readonly ILogger<GetPatientHabitQueryHandler> _logger;
        public GetPatientHabitQueryHandler(AppDbContext context, ILogger<GetPatientHabitQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Response<PatientHabit>> Handle(GetPatientHabitQuery request, CancellationToken cancellationToken)
        {
			try
			{
                var isPatientHabit = await _context.PatientHabit.FindAsync(request.Id);
                if (isPatientHabit is not null)
                {
                    return new Response<PatientHabit>
                    {
                        Succeeded = true,
                        Data = isPatientHabit
                    };
                }
                return new Response<PatientHabit>
                {
                    Succeeded = false,
                    Message = "Could not find Patient Habit with specified id"
                };
            }
			catch (Exception ex)
			{
                _logger.LogError(ex.Message);
                return new Response<PatientHabit>
                {
                    Succeeded = false,
                    Message = "Something went wrong"
                };
            }
        }
    }
}
