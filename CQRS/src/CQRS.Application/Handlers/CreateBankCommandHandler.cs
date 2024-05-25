using CQRS.Application.Commands;
using CQRS.Application.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Data.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Handlers
{
    public class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, BankDto>
    {
        private readonly IBankRepository _repository;

        public CreateBankCommandHandler(IBankRepository repository)
        {
            _repository = repository;
        }

        public async Task<BankDto> Handle(CreateBankCommand request, CancellationToken cancellationToken)
        {
            var bank = new Bank(request.Name,request.Bic,request.Country);
            await _repository.AddAsync(bank);
            return bank.Adapt<BankDto>();
        }
    }
}
