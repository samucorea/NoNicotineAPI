using NoNicotine_Business.Value_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Business.Repositories
{
    public interface IPatientConsumptionMethodsRepository
    {
        public Task<ConsumptionExpensesResponse?> CalculateDailyConsumption(string patientConsumptionMethodsId, CancellationToken cancellationToken);
    }
}
