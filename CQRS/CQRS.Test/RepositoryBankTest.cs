using System.Net;
using System.Text;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Data.Context;
using CQRS.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace CQRS.Test
{
    /// <summary>
    /// BankRepository tests against a real EF Core in-memory database (no mocks of DbSet,
    /// which cannot be mocked reliably). The external bank API is replaced by a stub HTTP handler.
    /// </summary>
    public class RepositoryBankTest
    {
        private sealed class StubHandler(string json) : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
                Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });
        }

        private sealed class StubHttpClientFactory(HttpClient client) : IHttpClientFactory
        {
            public HttpClient CreateClient(string name) => client;
        }

        private static (BankRepository Repository, ApplicationDbContext Context) Create(string externalApiJson = "[]")
        {
            var factory = new StubHttpClientFactory(new HttpClient(new StubHandler(externalApiJson)));
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options, factory);
            var repository = new BankRepository(
                context,
                factory,
                NullLogger<BankRepository>.Instance,
                Options.Create(new BankApiOptions { BaseUrl = "https://example.invalid/banks" }));

            return (repository, context);
        }

        private static Bank NewBank(string bic, string name = "Test Bank", string country = "ES") =>
            new Bank(name: name, bic: bic, country: country);

        [Fact]
        public async Task AddAsync_saves_the_bank_and_stamps_the_audit_fields()
        {
            var (repository, context) = Create();

            await repository.AddAsync(NewBank("BIC00001"));

            var saved = await context.Banks.FindAsync("BIC00001");
            Assert.NotNull(saved);
            Assert.Equal("User", saved!.CreatedBy);
            Assert.NotEqual(default, saved.CreatedDateTime);
            Assert.NotNull(saved.UpdatedDateTime);
        }

        [Fact]
        public async Task GetByIdAsync_returns_the_bank_when_it_exists()
        {
            var (repository, context) = Create();
            context.Banks.Add(NewBank("BIC00002", name: "Banco Dos"));
            await context.SaveChangesAsync();

            var result = await repository.GetByIdAsync("BIC00002");

            Assert.NotNull(result);
            Assert.Equal("Banco Dos", result!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_returns_null_when_it_does_not_exist()
        {
            var (repository, _) = Create();

            var result = await repository.GetByIdAsync("MISSING");

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_removes_an_existing_bank()
        {
            var (repository, context) = Create();
            context.Banks.Add(NewBank("BIC00003"));
            await context.SaveChangesAsync();

            await repository.DeleteAsync("BIC00003");

            Assert.Empty(context.Banks);
        }

        [Fact]
        public async Task DeleteAsync_of_an_unknown_bank_does_not_throw()
        {
            var (repository, context) = Create();
            context.Banks.Add(NewBank("BIC00004"));
            await context.SaveChangesAsync();

            await repository.DeleteAsync("MISSING");

            Assert.Single(context.Banks);
        }

        [Fact]
        public async Task GetAllAsync_paginates_the_stored_banks()
        {
            var (repository, context) = Create();
            for (var i = 1; i <= 5; i++)
            {
                context.Banks.Add(NewBank($"BIC1000{i}"));
            }
            await context.SaveChangesAsync();

            var all = await repository.GetAllAsync(null, null);
            var secondPage = await repository.GetAllAsync(2, 2);
            var lastPage = await repository.GetAllAsync(3, 2);

            Assert.Equal(5, all.Count);
            Assert.Equal(2, secondPage.Count);
            Assert.Single(lastPage);
        }

        [Fact]
        public async Task GetAllAsync_imports_from_the_external_api_when_the_store_is_empty()
        {
            const string json = """
                [
                  { "name": "Banco Uno", "bic": "EXT00001", "country": "ES" },
                  { "name": "Banco Dos", "bic": "EXT00002", "country": "PT" }
                ]
                """;
            var (repository, context) = Create(json);

            var result = await repository.GetAllAsync(null, null);

            Assert.Equal(2, result.Count);
            Assert.All(context.Banks, b => Assert.Equal("System", b.CreatedBy));
        }

        [Fact]
        public async Task UpdateAsync_saves_the_changes_and_stamps_the_audit_fields()
        {
            var (repository, context) = Create();
            var bank = NewBank("BIC00005", name: "Antes");
            context.Banks.Add(bank);
            await context.SaveChangesAsync();

            bank.Name = "Despues";
            await repository.UpdateAsync(bank);

            var saved = await context.Banks.FindAsync("BIC00005");
            Assert.Equal("Despues", saved!.Name);
            Assert.Equal("User", saved.UpdatedBy);
            Assert.NotNull(saved.UpdatedDateTime);
        }
    }
}
