using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public record RegisterDto
(
    [property: Required, StringLength(80, MinimumLength = 1)] string Name,
    [property: Required, EmailAddress, StringLength(254)] string Email,
    [property: Required, StringLength(80, MinimumLength = 8)] string Password
);