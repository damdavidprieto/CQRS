using CQRS.Application.DTOs;
using CQRS.Application.Queries;
using CQRS.Domain.Exceptions;
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

public class GetBankByIdQueryHandler : IRequestHandler<GetBankByIdQuery, ResultDto<BankDto>>
{
    private readonly IBankRepository _repository;
    private readonly ILogger<GetBankByIdQueryHandler> _logger;

    public GetBankByIdQueryHandler(IBankRepository repository, ILogger<GetBankByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ResultDto<BankDto>> Handle(GetBankByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new ResultDto<BankDto>();
        var Bank = await _repository.GetByIdAsync(request.Bic);
        if (Bank == null)
        {
            _logger.LogWarning("Bank with BIC {Bic} was not found.", request.Bic);
            throw new NotFoundException("Bank not found.");
        }
        _logger.LogInformation("Retrieved bank with BIC {Bic}.", request.Bic);
        result.Data = Bank.Adapt<BankDto>();
        return result;
    }
}