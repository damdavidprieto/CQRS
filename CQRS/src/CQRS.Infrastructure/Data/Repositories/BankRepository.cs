using CQRS.Domain.Entities;
using CQRS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CQRS.Infrastructure.Data.Repositories
{
    public class BankApiOptions
    {
        public string BaseUrl { get; set; }
    }
    public interface IBankRepository
    {
        Task AddAsync(Bank todo);
        Task<List<Bank>> GetAllAsync(int? pageNumber, int? pageSize);
        Task<Bank?> GetByIdAsync(string Bic);
        Task UpdateAsync(Bank todo);
        Task DeleteAsync(string Bic);
    }
    public class BankRepository : IBankRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BankRepository> _logger;
        private readonly string _bankApiBaseUrl;

        public BankRepository(ApplicationDbContext context, IHttpClientFactory httpClientFactory, ILogger<BankRepository> logger, IOptions<BankApiOptions> bankApiOptions)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _bankApiBaseUrl = bankApiOptions.Value.BaseUrl;
        }
        public async Task AddAsync(Bank bank)
        {
            try
            {
                bank.CreatedBy = "User";
                await _context.Banks.AddAsync(bank);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Added a new bank with BIC: {Bic}", bank.Bic);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error adding a new bank with BIC: {Bic}", bank.Bic);
                throw e;
            }
        }

        public async Task DeleteAsync(string Bic)
        {
            try
            {
                var bank = await _context.Banks.FindAsync(Bic);
                if (bank != null)
                {
                    _context.Banks.Remove(bank);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Deleted bank with BIC: {Bic}", Bic);
                }
                else
                {
                    _logger.LogWarning("Attempted to delete bank with BIC: {Bic}, but it was not found", Bic);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error deleting bank with BIC: {Bic}", Bic);
                throw e;
            }
        }

        public async Task<List<Bank>> GetAllAsync(int? pageNumber, int? pageSize)
        {
            try
            {
                if (!_context.Banks.Any())
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetFromJsonAsync<List<Bank>>(_bankApiBaseUrl);
                    foreach (Bank bank in response)
                    {
                        bank.CreatedBy = "System";
                        await _context.Banks.AddAsync(bank);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Added bank from external source with BIC: {Bic}", bank.Bic);
                    }
                }
                var result = await _context.Banks.ToListAsync();
                _logger.LogInformation("Retrieved {Count} banks", result.Count);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving all banks");
                throw e;
            }
        }

        public async Task<Bank?> GetByIdAsync(string Bic)
        {
            try
            {
                var bank = await _context.Banks.FindAsync(Bic);
                if (bank != null)
                {
                    _logger.LogInformation("Retrieved bank with BIC: {Bic}", Bic);
                }
                else
                {
                    _logger.LogWarning("Bank with BIC: {Bic} was not found", Bic);
                }
                return await _context.Banks.FindAsync(Bic);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving bank with BIC: {Bic}", Bic);
                throw e;
            }
        }

        public async Task UpdateAsync(Bank bank)
        {
            try
            {
                bank.UpdatedBy = "User";
                _context.Entry(bank).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated bank with BIC: {Bic}", bank.Bic);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating bank with BIC: {Bic}", bank.Bic);
                throw e;
            }
        }
    }
}
