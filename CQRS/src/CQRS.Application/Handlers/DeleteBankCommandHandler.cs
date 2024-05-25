using CQRS.Application.Commands;
using CQRS.Infrastructure.Data.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Handlers;
public class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand>
{
    private readonly IBankRepository _repository;

    public DeleteBankCommandHandler(IBankRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteBankCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Bic);
    }
}
