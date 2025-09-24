public interface IAuthService
{
    Task<RegistrationResult> RegisterAsync(RegisterDto dto, CancellationToken ct);
    Task<LoginResult> LoginAsync(LoginDto dto, CancellationToken ct);
    Task ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken ct);
}
