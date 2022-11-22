﻿using MediatR;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Commands.Create
{
    public class CreateCigarreteDetailsCommand : IRequest<Response<CigaretteDetails>>
    {
        public short unitsPerDay { get; set; }
        public short daysPerWeek { get; set; }
        public short unitsPerBox { get; set; }
        public decimal boxPrice { get; set; }
        public string PatientConsumptionMethodsId { get; set; }
    }
}