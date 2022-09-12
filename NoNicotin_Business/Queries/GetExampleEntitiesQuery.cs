using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NoNicotin_Business.Queries
{
    public class GetExampleEntitiesQuery : IRequest<List<ExampleEntity>>
    {
        public record GetProductsQuery() : IRequest<IEnumerable<ExampleEntity>>;
    }
}
