using Microsoft.EntityFrameworkCore;
using Moq;
using NoNicotine_Business.Handler;
using NoNicotine_Business.Queries;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using NoNicotineAPI.UnitTests.Mocks;
using Shouldly;


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

            var result = await handler.Handle(new GetPatientQuery() { UserId = "abcd" }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldNotBeNull();
            result.Data.ID.ShouldBe("abcd");
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldReturnNullIfPatientIsNotFoundWithAMessage()
        {
            var data = new List<Patient>
            {
                new Patient { Name = "BBB" },
                new Patient { Name = "ZZZ" },
                new Patient { Name = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Patient>>();
            mockSet.As<IQueryable<Patient>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Patient>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Patient>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Patient>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockDbContext.Setup(c => c.Patient).Returns(mockSet.Object);

            var handler = new GetPatientQueryHandler(_mockDbContext.Object);

            var result = await handler.Handle(new GetPatientQuery() { UserId = "a" }, CancellationToken.None);

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
