using Application.Services;
using DomainModel;
using DomainModel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("history")]
    public async Task<IActionResult> GetHistory([FromBody] GetHistoryRequest request)
    {
        try
        {
            IEnumerable<Transaction> transactions =
                await _transactionService.GetTransactionHistory(request.SessionKey);
            return Ok(new { transactions });
        }
        catch (SessionNotFoundException)
        {
            return Unauthorized(new { error = "Сессия не найдена" });
        }
    }
}
