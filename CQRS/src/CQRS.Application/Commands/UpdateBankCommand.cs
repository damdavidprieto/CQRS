using CQRS.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Commands;

public record UpdateBankCommand(string Bic, string Name, string Country) : IRequest<ResultDto<BankDto>>;