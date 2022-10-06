using Moq;
using NoNicotine_Business.Repositories;
using NoNicotine_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotineAPI.UnitTests.Mocks
{
    public static class MockPatientsRepository
    {
        public static Mock<IPatientsRepository> GetPatientsRepository()
        {
     

            var mockRepo = new Mock<IPatientsRepository>();

            return mockRepo;
        }
    }
}
