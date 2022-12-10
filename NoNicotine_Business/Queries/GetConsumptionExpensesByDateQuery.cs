using MediatR;
using NoNicotine_Business.Value_Objects;
using NoNicotineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Queries
{
    public class GetConsumptionExpensesByDateQuery : IRequest<Response<ConsumptionExpensesResponse>>
    {
        public string PatientId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
    }
}
