using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class UserAssignment
{
    [StringLength(128, MinimumLength = 5, ErrorMessageResourceName = "lengthErrorMessage",
        ErrorMessageResourceType = typeof(Base.Resources.Identity))]
    public string Email { get; set; } = default!;

    public string NewRole { get; set; } = default!;
}