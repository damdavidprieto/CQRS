﻿using CQRS.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.Commands;
public record CreateBankCommand(string Title, string Description) : IRequest<BankDto>;