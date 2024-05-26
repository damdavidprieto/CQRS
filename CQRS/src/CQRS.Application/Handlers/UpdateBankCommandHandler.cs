using CQRS.Application.Commands;
using CQRS.Application.DTOs;
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

public class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, ResultDto<BankDto>>
{
    private readonly IBankRepository _repository;
    private readonly ILogger<UpdateBankCommandHandler> _logger;

    public UpdateBankCommandHandler(IBankRepository repository, ILogger<UpdateBankCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ResultDto<BankDto>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        var result = new ResultDto<BankDto>();
        try
        {
            var bank = await _repository.GetByIdAsync(request.Bic);

            if (bank == null)
            {
                _logger.LogWarning("Bank with BIC: {Bic} not found.", request.Bic);
                throw new NotFoundException("Bank item not found.");
            }

            bank.Name = request.Name;
            bank.Bic = request.Bic; // Usually, BIC should not be updated as it is an identifier
            bank.Country = request.Country;

            await _repository.UpdateAsync(bank);
            _logger.LogInformation("Updated bank with BIC: {Bic}", bank.Bic);

            result.Data = bank.Adapt<BankDto>();
            result.Messages.Add("Bank updated successfully.");
        }
        catch (NotFoundException e)
        {
            _logger.LogWarning(e, "Bank with BIC: {Bic} not found.", request.Bic);
        
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while updating the bank with BIC: {Bic}", request.Bic);
        }
        return result;
    }
}