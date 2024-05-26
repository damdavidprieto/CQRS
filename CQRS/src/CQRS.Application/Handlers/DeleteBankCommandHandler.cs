using CQRS.Application.Commands;
using CQRS.Application.DTOs;
using CQRS.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Handlers;
public class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand, ResultDto>
{
    private readonly IBankRepository _repository;
    private readonly ILogger<DeleteBankCommandHandler> _logger;

    public DeleteBankCommandHandler(IBankRepository repository, ILogger<DeleteBankCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ResultDto> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
    {
        var result = new ResultDto();
        var bank = await _repository.GetByIdAsync(request.Bic);
        if (bank == null)
        {
            _logger.LogWarning("Attempted to delete bank with BIC {Bic}, but it was not found.", request.Bic);
            throw new KeyNotFoundException($"Bank with BIC {request.Bic} was not found.");
        }
        await _repository.DeleteAsync(request.Bic);
        _logger.LogInformation("Bank with BIC {Bic} was successfully deleted.", request.Bic);
        result.Menssages.Add("Bank was successfully deleted.");
        return result;
    }
}
