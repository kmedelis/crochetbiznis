public sealed record RegistrationResult(
    bool Succeeded,
    IDictionary<string, string[]>? Errors = null
)
{
    public static RegistrationResult Ok() => new(true, null);
    public static RegistrationResult Fail(IDictionary<string, string[]> errors) => new(false, errors);
}
