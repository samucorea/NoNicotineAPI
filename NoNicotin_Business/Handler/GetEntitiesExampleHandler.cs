using MediatR;
using NoNicotin_Business.Queries;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotin_Business.Handler
{
    public class GetEntitiesExampleHandler : IRequestHandler<GetExampleEntitiesQuery, IEnumerable<ExampleEntity>>
    {
        public Task<IEnumerable<ExampleEntity>> Handle(GetExampleEntitiesQuery request, CancellationToken cancellationToken)
        {
			try
			{
				// add logic here
				var entities = new List<ExampleEntity>();
				return (Task<IEnumerable<ExampleEntity>>)entities.AsEnumerable();
			}
			catch (Exception)
			{
				// Log error here
				throw;
			}
        }
    }
}
