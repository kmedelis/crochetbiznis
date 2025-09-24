using Crochetbiznis.Infrastructure.Identity;

public interface ITokenService
{
    Task<TokenResponse> CreateAccessTokenAsync(ApplicationUser user, CancellationToken ct);
}
