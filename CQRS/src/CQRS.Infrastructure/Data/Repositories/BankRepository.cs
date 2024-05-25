using CQRS.Domain.Entities;
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
        Task<Bank?> GetByIdAsync(Guid id);
        Task UpdateAsync(Bank todo);
        Task DeleteAsync(Guid id);
        Task<List<Bank>> GetPendingTodosAsync();
        Task<List<Bank>> GetCompletedTodosAsync();
    }
    public class BankRepository : IBankRepository
    {
        public Task AddAsync(Bank todo)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Bank>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Bank?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Bank>> GetCompletedTodosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Bank>> GetPendingTodosAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Bank todo)
        {
            throw new NotImplementedException();
        }
    }
}
