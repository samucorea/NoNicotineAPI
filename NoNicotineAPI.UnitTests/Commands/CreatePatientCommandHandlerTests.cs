using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoNicotine_Business.Commands;
using NoNicotine_Business.Handler;
using NoNicotine_Business.Repositories;
using NoNicotine_Business.Value_Objects;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.UnitTests.Mocks;
using Shouldly;


namespace NoNicotineAPI.UnitTests.Commands
{
    public class CreatePatientCommandHandlerTests
    {
        private readonly Mock<IPatientsRepository> _mockRepo;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<ILogger<CreatePatientCommandHandler>> _mockLogger;
        private readonly Mock<AppDbContext> _mockDbContext;

        public CreatePatientCommandHandlerTests()
        {
            _mockRepo = new Mock<IPatientsRepository>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _mockLogger = new Mock<ILogger<CreatePatientCommandHandler>>();
            _mockDbContext = MockDbContext.GetMockDbContext();
        }

        [Fact]

        public async Task CreatePatientAsyncTest_ShouldCreatePatient()
        { 
            var mockPatientSet = new Mock<DbSet<Patient>>();

            _mockDbContext.Setup(m => m.Patient).Returns(mockPatientSet.Object);


            _mockUserManager.Setup(s =>
            s.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())
            ).ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(s =>
            s.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())
            ).ReturnsAsync(IdentityResult.Success);


            var handler = new CreatePatientCommandHandler(_mockRepo.Object, _mockUserManager.Object, _mockLogger.Object, _mockDbContext.Object);

            var result = await handler.Handle(new CreatePatientCommand()
            {
                Name = "Samuel Garcia",
                BirthDate = new DateTime(1995, 5, 2),
                Identification = "123123123",
                Email = "hey@hotmail.com",
                Password = "12312312",
                IdentificationPatientType = IdentificationType.IDENTIFICATION_CARD,
                Sex = 'M'
            }, CancellationToken.None);

            result.Succeeded.ShouldBeTrue();
            result.Data.ShouldNotBeNull();
            result.Data.Name.ShouldBe("Samuel Garcia");

            mockPatientSet.Verify(m => m.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockUserManager.Verify(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
