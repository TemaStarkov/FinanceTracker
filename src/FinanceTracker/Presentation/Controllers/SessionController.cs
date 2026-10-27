using Application.Services;
using DomainModel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUserSession([FromBody] CreateUserSessionRequest request)
    {
        try
        {
            Guid sessionKey = await _sessionService.CreateUserSession(request.AccountNumber, request.PinCode);
            return Ok(new { sessionKey });
        }
        catch (AccountNotFoundException)
        {
            return BadRequest(new { error = "Счет не найден" });
        }
        catch (InvalidPinCodeException)
        {
            return BadRequest(new { error = "Неверный PIN-код" });
        }
    }

    [HttpPost("create-admin")]
    public async Task<IActionResult> CreateAdminSession([FromBody] CreateAdminSessionRequest request)
    {
        try
        {
            Guid sessionKey = await _sessionService.CreateAdminSession(request.SystemPassword);
            return Ok(new { sessionKey });
        }
        catch (InvalidPasswordException)
        {
            return BadRequest(new { error = "Неверный системный пароль" });
        }
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateSession([FromBody] ValidateSessionRequest request)
    {
        bool isValid = await _sessionService.ValidateSession(request.SessionKey);
        return Ok(new { isValid });
    }
}
