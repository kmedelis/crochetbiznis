using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Crochetbiznis.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    [StringLength(80)]
    public string? DisplayName { get; set; }
}
