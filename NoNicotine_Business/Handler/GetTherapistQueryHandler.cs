﻿using MediatR;
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
    public class GetTherapistQueryHandler : IRequestHandler<GetTherapistQuery, Response<Therapist>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<GetTherapistQueryHandler> _logger;
        public GetTherapistQueryHandler(AppDbContext context, ILogger<GetTherapistQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<Therapist>> Handle(GetTherapistQuery request, CancellationToken cancellationToken)
        {

            var response = ValidateRequest(request);
            if (response != null)
            {
                return response;
            }

            var therapist = await _context.Therapist.FindAsync(request.Id, cancellationToken);
            if (therapist == null)
            {
                return new Response<Therapist>
                {
                    Succeeded = false,
                    Message = "Could not find Therapist with specified id"
                };
            }

            return new Response<Therapist>
            {
                Succeeded = true,
                Data = therapist
            };

        }

        private static Response<Therapist>? ValidateRequest(GetTherapistQuery request)
        {
            if (request.Id == null || request.Id.Length == 0)
            {
                return new Response<Therapist>
                {
                    Succeeded = false,
                    Message = "Missing Therapist Id"
                };
            }

            return null;
        }
    }
}
