using Microsoft.EntityFrameworkCore;
using Moq;
using NoNicotine_Business.Handler.Get;
using NoNicotine_Business.Queries;
using NoNicotine_Business.Repositories;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.Models;
using NoNicotineAPI.UnitTests.Mocks;
using Shouldly;


namespace NoNicotineAPI.UnitTests.Queries
{
    public class GetPatientQueryHandlerTests
    {
        private readonly Mock<IPatientRepository> _mockPatientRepository;

        public GetPatientQueryHandlerTests()
        {
            _mockPatientRepository = new Mock<IPatientRepository>();
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldReturnPatientWithSpecifiedId()
        {

            _mockPatientRepository.Setup(r => r.GetPatientByUserIdAsync("abcd", CancellationToken.None)).ReturnsAsync(new PatientDTO() { ID = "abcd" });

            var handler = new GetPatientQueryHandler(_mockPatientRepository.Object);

            var result = await handler.Handle(new GetPatientQuery() { UserId = "abcd" }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldNotBeNull();
            result.Data.ID.ShouldBe("abcd");
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldReturnNullIfPatientIsNotFoundWithAMessage()
        {

            var handler = new GetPatientQueryHandler(_mockPatientRepository.Object);

            var result = await handler.Handle(new GetPatientQuery() { UserId = "a" }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldBeNull();
            result.Succeeded.ShouldBeFalse();
            result.Message.ShouldBe("Could not find Patient with specified id");
        }

        [Fact]

        public async Task GetPatientByIdTest_ShouldFailIfIdIsNotSpecified()
        {

            var handler = new GetPatientQueryHandler(_mockPatientRepository.Object);

            var result = await handler.Handle(new GetPatientQuery() { }, CancellationToken.None);

            result.ShouldBeOfType<Response<Patient>>();

            result.Data.ShouldBeNull();
            result.Succeeded.ShouldBeFalse();
            result.Message.ShouldBe("Missing Patient Id");
        }
    }
}
