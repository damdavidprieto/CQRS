using CQRS.Application.Commands;
using CQRS.Application.DTOs;
using CQRS.Domain.Exceptions;
using CQRS.Infrastructure.Data.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Handlers;

public class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, BankDto>
{
    private readonly IBankRepository _repository;

    public UpdateBankCommandHandler(IBankRepository repository)
    {
        _repository = repository;
    }

    public async Task<BankDto> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        var Bank = await _repository.GetByIdAsync(request.Bic);

        if (Bank == null) throw new NotFoundException("Bank item not found.");

        Bank.Name = request.Name;
        Bank.Bic = request.Bic;
        Bank.Country = request.Country;

        await _repository.UpdateAsync(Bank);
        return Bank.Adapt<BankDto>();
    }
}