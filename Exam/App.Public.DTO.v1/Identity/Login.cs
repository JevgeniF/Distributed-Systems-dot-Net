using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Login
{
    [StringLength(128, MinimumLength = 5, ErrorMessageResourceName = "lengthErrorMessage",
        ErrorMessageResourceType = typeof(Base.Resources.Identity))]
    public string Email { get; set; } = default!;

    [StringLength(24, MinimumLength = 6, ErrorMessageResourceName = "lengthErrorMessage",
        ErrorMessageResourceType = typeof(Base.Resources.Identity))]
    public string Password { get; set; } = default!;
}