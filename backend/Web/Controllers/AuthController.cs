// Web/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        var res = await _auth.RegisterAsync(dto, ct);
        if (res.Succeeded) return StatusCode(201);

        foreach (var (key, messages) in res.Errors!)
            foreach (var msg in messages)
                ModelState.AddModelError(key, msg);

        return ValidationProblem(ModelState); 
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var res = await _auth.LoginAsync(dto, ct);
        if (!res.Succeeded) return Unauthorized(); // 401

        return Ok(res.Token);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto, CancellationToken ct)
    {
        await _auth.ForgotPasswordAsync(dto, ct);
        return NoContent();
    }
}
