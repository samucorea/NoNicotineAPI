using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using NoNicotine_Data.Context;
using NoNicotine_Data.Entities;
using NoNicotineAPI.UnitTests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotineAPI.UnitTests.Mocks
{
    public static class MockDbContext
    {
        public static Mock<AppDbContext> GetMockDbContext()
        {
            var mockDbContext = new Mock<AppDbContext>();

            var databaseFacadeMock = new Mock<DatabaseFacade>(mockDbContext.Object);

            mockDbContext.SetupGet(x => x.Database).Returns(databaseFacadeMock.Object);

            mockDbContext.Setup(m => m.Database.BeginTransaction()).Returns(new DbContextTransactionMock());

            mockDbContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() =>
            {
                return 1;
            });

            return mockDbContext;
        }
    }
}
