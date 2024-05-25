using CQRS.Application.DTOs;
using CQRS.Application.Queries;
using CQRS.Infrastructure.Data.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Handlers;

public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, IEnumerable<BankDto>>
{
    private readonly IBankRepository _repository;

    public GetBanksQueryHandler(IBankRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BankDto>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
    {
        var Banks = await _repository.GetAllAsync();
        return Banks.Adapt<IEnumerable<BankDto>>();
    }
}