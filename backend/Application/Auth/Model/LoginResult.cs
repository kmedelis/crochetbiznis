public sealed record LoginResult(bool Succeeded, TokenResponse? Token = null)
{
    public static LoginResult Ok(TokenResponse token) => new(true, token);
    public static LoginResult Fail() => new(false, null);
}