using CQRS.Domain.Entities;
using CQRS.Infrastructure.Data.Context;
using CQRS.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CQRS.Test
{
    public class RepositoryBankTest
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<BankRepository>> _mockLogger;
        private readonly IOptions<BankApiOptions> _bankApiOptions;

        public RepositoryBankTest()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
           
            _mockContext = new Mock<ApplicationDbContext>(options);
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<BankRepository>>();
            _bankApiOptions = Options.Create(new BankApiOptions { BaseUrl = "https://api.opendata.esett.com/EXP06/Banks" });
        }

        private BankRepository CreateRepository()
        {
            return new BankRepository(_mockContext.Object, _mockHttpClientFactory.Object, _mockLogger.Object, _bankApiOptions);
        }

        [Fact]
        public async Task AddAsync_ShouldAddBank()
        {
            // Arrange
            var repository = CreateRepository();
            var bank = new Bank ("TESTBIC", "Test Bank", "Test Country" );

            // Act
            await repository.AddAsync(bank);

            // Assert
            _mockContext.Verify(m => m.Banks.AddAsync(It.IsAny<Bank>(), default), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteBank()
        {
            // Arrange
            var repository = CreateRepository();
            var bank = new Bank("TESTBIC", "Test Bank", "Test Country");

            _mockContext.Setup(m => m.Banks.FindAsync(It.IsAny<string>())).ReturnsAsync(bank);

            // Act
            await repository.DeleteAsync("TESTBIC");

            // Assert
            _mockContext.Verify(m => m.Banks.Remove(It.IsAny<Bank>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnBanks()
        {
            // Arrange
            var repository = CreateRepository();
            var banks = new List<Bank>
            {
                new Bank  ("TESTBIC", "Test Bank", "Test Country" ),
                new Bank    ("TESTBIC2", "Test Bank2", "Test Country2")
        };

            _mockContext.Setup(m => m.Banks.Any()).Returns(true);
            _mockContext.Setup(m => m.Banks.ToListAsync(default)).ReturnsAsync(banks);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBank()
        {
            // Arrange
            var repository = CreateRepository();
            var bank = new Bank("TESTBIC2", "Test Bank2", "Test Country2");

            _mockContext.Setup(m => m.Banks.FindAsync(It.IsAny<string>())).ReturnsAsync(bank);

            // Act
            var result = await repository.GetByIdAsync("TESTBIC");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TESTBIC", result?.Bic);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBank()
        {
            // Arrange
            var repository = CreateRepository();
            var bank = new Bank("TESTBIC2", "Test Bank2", "Test Country2");

            // Act
            await repository.UpdateAsync(bank);

            // Assert
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }
    }
}
