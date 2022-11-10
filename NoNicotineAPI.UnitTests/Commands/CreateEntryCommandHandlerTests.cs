using Moq;
using NoNicotine_Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotineAPI.UnitTests.Commands
{
    public class CreateEntryCommandHandlerTests
    {
        private readonly Mock<IEntryRepository> _mockEntryRepository;

        public CreateEntryCommandHandlerTests()
        {
            _mockEntryRepository = new Mock<IEntryRepository>();
        }

  
    }
}
