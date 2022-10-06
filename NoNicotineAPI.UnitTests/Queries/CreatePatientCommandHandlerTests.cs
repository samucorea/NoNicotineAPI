//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NoNicotine_Business.Commands;
//using NoNicotine_Business.Handler;
//using NoNicotine_Business.Repositories;
//using NoNicotine_Business.Value_Objects;
//using NoNicotine_Data.Context;
//using NoNicotine_Data.Entities;
//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NoNicotineAPI.UnitTests.Queries
//{
//    public class CreatePatientCommandHandlerTests
//    {
//        private readonly Mock<IPatientsRepository> _mockRepo;
//        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
//        private readonly Mock<ILogger<CreatePatientCommandHandler>> _mockLogger;

//        public CreatePatientCommandHandlerTests()
//        {
//            _mockRepo = new Mock<IPatientsRepository>();
//            _mockUserManager = new Mock<UserManager<IdentityUser>>(null);
//            _mockLogger = new Mock<ILogger<CreatePatientCommandHandler>>();
//        }

//        [Fact]

//        public async Task CreatePatientAsyncTest_ShouldCreatePatient()
//        {

//            var patients = new List<Patient>()
//            {
//                new Patient()
//                {
//                    ID = "abcd",
//                    Name = "Django Desencadenado"
//                },
//                 new Patient()
//                {
//                    ID = "abcd",
//                    Name = "Django Desencadenado"
//                },
//                  new Patient()
//                {
//                    ID = "abcd",
//                    Name = "Django Desencadenado"
//                },
//                   new Patient()
//                {
//                    ID = "abcd",
//                    Name = "Django Desencadenado"
//                },
//            };
//            _mockRepo.Setup(r => r.CreatePatientAsync(It.IsAny<Patient>())).ReturnsAsync((Patient patient) =>
//            {
//                patients.Add(patient);

//                return 1;
//            });

//            var handler = new CreatePatientCommandHandler(_mockRepo.Object, _mockUserManager.Object, _mockLogger.Object);

//            var result = await handler.Handle(new CreatePatientCommand()
//            {
//                Name = "Samuel Garcia",
//                BirthDate = new DateTime(1995, 5, 2),
//                Identification = "123123123",
//                Email = "hey@hotmail.com",
//                Password = "12312312",
//                IdentificationPatientType = IdentificationType.IDENTIFICATION_CARD,
//                Sex = 'M'
//            }, CancellationToken.None);

//            result.Succeeded.ShouldBeTrue();
//            result.Data.ShouldNotBeNull();
//            result.Data.Name.ShouldBe("Samuel Garcia");

//            patients.Count.ShouldBe(5);


//        }
//    }
//}
