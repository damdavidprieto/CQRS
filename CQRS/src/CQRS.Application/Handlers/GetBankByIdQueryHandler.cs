using CQRS.Application.DTOs;
using CQRS.Application.Queries;
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

public class GetBankByIdQueryHandler : IRequestHandler<GetBankByIdQuery, BankDto>
{
    private readonly IBankRepository _repository;

    public GetBankByIdQueryHandler(IBankRepository repository)
    {
        _repository = repository;
    }

    public async Task<BankDto> Handle(GetBankByIdQuery request, CancellationToken cancellationToken)
    {
        var Bank = await _repository.GetByIdAsync(request.Bic);
        if (Bank == null) throw new NotFoundException("Bank not found.");
        return Bank.Adapt<BankDto>();
    }
}