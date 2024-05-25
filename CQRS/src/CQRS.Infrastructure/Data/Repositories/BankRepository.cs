using CQRS.Domain.Entities;
using CQRS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BankRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Bank todo)
        {
            await _context.Banks.AddAsync(todo);
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
