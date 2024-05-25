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

namespace CQRS.Infrastructure.Data.Repositories
{
    public interface IBankRepository
    {
        Task AddAsync(Bank todo);
        Task<List<Bank>> GetAllAsync();
        Task<Bank?> GetByIdAsync(string Bic);
        Task UpdateAsync(Bank todo);
        Task DeleteAsync(string Bic);
    }
    public class BankRepository : IBankRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public BankRepository(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            //Seed().GetAwaiter();
        }
        public async Task AddAsync(Bank bank)
        {
            await _context.Banks.AddAsync(bank);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string Bic)
        {
            var bank = await _context.Banks.FindAsync(Bic);
            if (bank != null)
            {
                _context.Banks.Remove(bank);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Bank>> GetAllAsync()
        {
            if (!_context.Banks.Any())
            { 
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<List<Bank>>("https://api.opendata.esett.com/EXP06/Banks");
                foreach (Bank bank in response)
                {
                    await _context.Banks.AddAsync(bank);
                    await _context.SaveChangesAsync();
                }
            }
            var result = await _context.Banks.ToListAsync();
            return result;
        }

        public async Task<Bank?> GetByIdAsync(string Bic)
        {
            return await _context.Banks.FindAsync(Bic);
        }

        public async Task UpdateAsync(Bank bank)
        {
            _context.Entry(bank).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
