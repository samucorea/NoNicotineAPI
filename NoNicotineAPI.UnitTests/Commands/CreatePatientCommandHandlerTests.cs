using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoNicotine_Business.Commands.Create;
using NoNicotine_Business.Handler.Create;
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
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<ILogger<CreatePatientCommandHandler>> _mockLogger;
        private readonly Mock<IPatientRepository> _mockPatientRepository;
        private readonly Mock<AppDbContext> _mockDbContext;

        public CreatePatientCommandHandlerTests()
        { 
            _mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _mockLogger = new Mock<ILogger<CreatePatientCommandHandler>>();
            _mockDbContext = MockDbContext.GetMockDbContext();
            _mockPatientRepository = new Mock<IPatientRepository>();
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

            _mockPatientRepository.Setup(r => r.CreatePatientAsync(It.IsAny<Patient>(), CancellationToken.None)).ReturnsAsync(1);


            var handler = new CreatePatientCommandHandler(_mockUserManager.Object, _mockLogger.Object,_mockPatientRepository.Object, _mockDbContext.Object);

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
        }
    }
}
