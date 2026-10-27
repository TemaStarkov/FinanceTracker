using Application.Services;
using DomainModel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
    {
        try
        {
            await _accountService.Withdraw(request.SessionKey, request.Amount);
            return Ok(new { message = "Снятие выполнено успешно" });
        }
        catch (SessionNotFoundException)
        {
            return Unauthorized(new { error = "Сессия не найдена" });
        }
        catch (AccountNotFoundException)
        {
            return BadRequest(new { error = "Счет не найден" });
        }
        catch (InvalidAmountException)
        {
            return BadRequest(new { error = "Сумма должна быть положительной" });
        }
        catch (InsufficientFundsException)
        {
            return BadRequest(new { error = "Недостаточно средств на счете" });
        }
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
    {
        try
        {
            await _accountService.Deposit(request.SessionKey, request.Amount);
            return Ok(new { message = "Пополнение выполнено успешно" });
        }
        catch (SessionNotFoundException)
        {
            return Unauthorized(new { error = "Сессия не найдена" });
        }
        catch (AccountNotFoundException)
        {
            return BadRequest(new { error = "Счет не найден" });
        }
        catch (InvalidAmountException)
        {
            return BadRequest(new { error = "Сумма должна быть положительной" });
        }
    }

    [HttpPost("balance")]
    public async Task<IActionResult> GetBalance([FromBody] GetBalanceRequest request)
    {
        try
        {
            decimal balance = await _accountService.GetBalance(request.SessionKey);
            return Ok(new { balance });
        }
        catch (SessionNotFoundException)
        {
            return Unauthorized(new { error = "Сессия не найдена" });
        }
        catch (AccountNotFoundException)
        {
            return BadRequest(new { error = "Счет не найден" });
        }
    }
}
