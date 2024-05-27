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
        try
        {
            var bank = await _repository.GetByIdAsync(request.Bic);
            if (bank == null)
            {
                _logger.LogWarning("Bank with BIC {Bic} was not found.", request.Bic);
                result.Messages.Add("Bank not found.");
                result.Code = 1; // Error code for not found
                return result;
            }

            _logger.LogInformation("Retrieved bank with BIC {Bic}.", request.Bic);
            result.Data = bank.Adapt<BankDto>();
            result.Code = 0; // Success code
            result.Messages.Add("Bank successfully retrieved.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the bank with BIC {Bic}.", request.Bic);
            result.Messages.Add("An error occurred while retrieving the bank.");
            result.Code = -1; // General error code
        }
        return result;
    }
}