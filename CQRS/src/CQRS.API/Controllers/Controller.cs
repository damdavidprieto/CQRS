using CQRS.Application.Commands;
using CQRS.Application.DTOs;
using CQRS.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class Controller : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<ActionResult<BankDto>> CreateBank(CreateBankCommand command)
    {
        var Bank = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBankById), new { Bic = Bank.Bic }, Bank);
    }

    [HttpGet("{Bic:string}")]
    public async Task<ActionResult<BankDto>> GetBankById(string Bic)
    {
        var Bank = await _mediator.Send(new GetBankByIdQuery(Bic));
        return Ok(Bank);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankDto>>> GetBanks()
    {
        var Banks = await _mediator.Send(new GetBanksQuery());
        return Ok(Banks);
    }

    [HttpPut("{Bic:string}")]
    public async Task<IActionResult> UpdateBank(string Bic, UpdateBankCommand command)
    {
        if (Bic != command.Bic) return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{Bic:string}")]
    public async Task<IActionResult> DeleteBank(string Bic)
    {
        await _mediator.Send(new DeleteBankCommand(Bic));
        return NoContent();
    }
}