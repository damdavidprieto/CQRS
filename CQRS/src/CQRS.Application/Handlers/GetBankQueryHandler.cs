using CQRS.Application.DTOs;
using CQRS.Application.Queries;
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

public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, ResultDto<IEnumerable<BankDto>>>
{
    private readonly IBankRepository _repository;
    private readonly ILogger<GetBanksQueryHandler> _logger;

    public GetBanksQueryHandler(IBankRepository repository, ILogger<GetBanksQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ResultDto<IEnumerable<BankDto>>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
    {
        var result = new ResultDto<IEnumerable<BankDto>>();
        try
        {
            var banks = await _repository.GetAllAsync();
            result.Data = banks.Adapt<IEnumerable<BankDto>>();
            _logger.LogInformation("Retrieved {Count} banks.", result.Data.Count());
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while retrieving all banks.");
            return result;
        }
    }
}