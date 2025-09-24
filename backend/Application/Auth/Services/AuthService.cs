using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Crochetbiznis.Infrastructure.Identity;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly ITokenService _tokens;

    public AuthService(UserManager<ApplicationUser> users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<RegistrationResult> RegisterAsync(RegisterDto dto, CancellationToken ct)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var name = dto.Name.Trim();

        var user = new ApplicationUser { UserName = email, Email = email, DisplayName = name };

        var result = await _users.CreateAsync(user, dto.Password);
        if (result.Succeeded) return RegistrationResult.Ok();

        var errors = result.Errors
            .GroupBy(e => MapKey(e))
            .ToDictionary(g => g.Key, g => g.Select(x => x.Description).ToArray());

        return RegistrationResult.Fail(errors);
    }

    public async Task<LoginResult> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await _users.FindByEmailAsync(email);
        if (user is null) return LoginResult.Fail();

        var ok = await _users.CheckPasswordAsync(user, dto.Password);
        if (!ok) return LoginResult.Fail();

        var token = await _tokens.CreateAccessTokenAsync(user, ct);
        return LoginResult.Ok(token);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var user = await _users.FindByEmailAsync(email);

        if (user is null) return;

        var resetToken = await _users.GeneratePasswordResetTokenAsync(user);

        // TODO: send email with a link like:
        // https://your-frontend/reset-password?email=<url-encoded>&token=<url-encoded>
        // For security, DO NOT return the token in the API response.
        _ = resetToken; // suppress unused warning
    }

    private static string MapKey(IdentityError e) =>
        e.Code switch
        {
            "DuplicateEmail" => "email",
            var c when c.Contains("Password") => "password",
            var c when c.Contains("UserName") => "email",
            _ => string.Empty
        };
}
