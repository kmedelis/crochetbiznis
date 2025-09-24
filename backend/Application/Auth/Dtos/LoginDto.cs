using System.ComponentModel.DataAnnotations;


public record LoginDto(
    [property: Required, EmailAddress, StringLength(254)] string Email,
    [property: Required, StringLength(100, MinimumLength = 8)] string Password
);