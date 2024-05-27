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

public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, ResultListDto<BankDto>>
{
    private readonly IBankRepository _repository;
    private readonly ILogger<GetBanksQueryHandler> _logger;

    public GetBanksQueryHandler(IBankRepository repository, ILogger<GetBanksQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ResultListDto<BankDto>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
    {
        var result = new ResultListDto<BankDto>();
        try
        {
            var banks = await _repository.GetAllAsync(request.PageNumber,request.PageSize);
            result.Data = new PaginationDto<BankDto>();
            result.Data.TotalItems = banks.Count;
            result.Data.Items = banks.Adapt<IEnumerable<BankDto>>();
            result.Code = 0; // Success code
            result.Messages.Add($"Retrieved {result.Data.TotalItems} banks.");
            _logger.LogInformation("Retrieved {Count} banks.", result.Data.TotalItems);
        }
        catch (Exception e)
        {
            result.Code = -1; // Error code
            result.Messages.Add("An error occurred while retrieving all banks.");
            _logger.LogError(e, "An error occurred while retrieving all banks.");
        }
        return result;
    }
}