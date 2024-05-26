using CQRS.Application.Commands;
using CQRS.Application.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Data.Repositories;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Handlers;
public class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, BankDto>
{
    private readonly IBankRepository _repository;
    private readonly ILogger<CreateBankCommandHandler> _logger;

    public CreateBankCommandHandler(IBankRepository repository, ILogger<CreateBankCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<BankDto> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        var bank = new Bank(request.Name,request.Bic,request.Country);
        await _repository.AddAsync(bank);
        var bankDto = bank.Adapt<BankDto>();
        bankDto.Code = 0;
        bankDto.Menssages.Add("Bank successfully created.");
        _logger.LogInformation("A new bank with BIC {Bic} was successfully created.", bank.Bic);
        return bank.Adapt<BankDto>();
    }
}

