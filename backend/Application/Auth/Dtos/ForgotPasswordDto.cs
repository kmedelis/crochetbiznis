using System.ComponentModel.DataAnnotations;

public record ForgotPasswordDto([property: Required, EmailAddress] string Email);