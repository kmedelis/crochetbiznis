// Application/Auth/Services/ITokenService.cs
using System.Threading;
using System.Threading.Tasks;
using Crochetbiznis.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public sealed class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly string _issuer, _audience;
    private readonly SymmetricSecurityKey _key;
    private const int AccessMinutes = 15;

    public TokenService(UserManager<ApplicationUser> users, IConfiguration cfg)
    {
        _users = users;
        _issuer = cfg["Jwt:Issuer"]!;
        _audience = cfg["Jwt:Audience"]!;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
    }

    public async Task<TokenResponse> CreateAccessTokenAsync(ApplicationUser user, CancellationToken ct)
    {
        var roles = await _users.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(AccessMinutes),
            signingCredentials: creds
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new TokenResponse(token, AccessMinutes * 60);
    }
}
