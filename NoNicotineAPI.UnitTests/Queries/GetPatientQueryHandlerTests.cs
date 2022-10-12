 using Moq;
using NoNicotine_Business.Handler;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using NoNicotineAPI.UnitTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotineAPI.UnitTests.Queries
{
    public class GetPatientQueryHandlerTests
    {
        private readonly Mock<AppDbContext> _mockDbContext;

        public GetPatientQueryHandlerTests()
        {
            _mockDbContext = MockDbContext.GetMockDbContext();
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldReturnPatientWithSpecifiedId()
        {

            _mockDbContext.Setup(x => x.Patient.FindAsync("abcd")).ReturnsAsync(new Patient()
            {
                ID = "abcd",
                Name = "Samuel Garcia"
            });

            var handler = new GetPatientQueryHandler(_mockDbContext.Object);

            var result = await handler.Handle(new GetPatientQuery() { Id = "abcd" }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldNotBeNull();
            result.Data.ID.ShouldBe("abcd");
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldReturnNullIfPatientIsNotFoundWithAMessage()
        {

            _mockDbContext.Setup(x => x.Patient.FindAsync("abcd")).ReturnsAsync(new Patient()
            {
                ID = "abcd",
                Name = "Samuel Garcia"
            });


            var handler = new GetPatientQueryHandler(_mockDbContext.Object);

            var result = await handler.Handle(new GetPatientQuery() { Id = "a" }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldBeNull();
            result.Succeeded.ShouldBeFalse();
            result.Message.ShouldBe("Could not find Patient with specified id");
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldFailIfIdIsNotSpecified()
        {

            var handler = new GetPatientQueryHandler(_mockDbContext.Object);

            var result = await handler.Handle(new GetPatientQuery() { }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldBeNull();
            result.Succeeded.ShouldBeFalse();
            result.Message.ShouldBe("Missing Patient Id");
        }
    }
}
