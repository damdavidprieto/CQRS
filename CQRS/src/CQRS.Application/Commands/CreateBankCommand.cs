using CQRS.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Commands;
public record CreateBankCommand(string Name, string Bic, string Country) : IRequest<BankDto>;
