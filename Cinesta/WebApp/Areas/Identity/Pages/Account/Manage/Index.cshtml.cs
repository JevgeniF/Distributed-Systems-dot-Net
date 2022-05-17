// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using App.Contracts.DAL;
using App.Domain.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IAppUOW _uow;
    private readonly UserManager<AppUser> _userManager;

    public IndexModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IAppUOW uow)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _uow = uow;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>

    public string Username { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    private async Task LoadAsync(AppUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        var name = User.GetUserName();
        var surname = User.GetUserSurname();

        Username = userName;

        Input = new InputModel
        {
            Name = name,
            Surname = surname,
            PhoneNumber = phoneNumber
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        var person = await _uow.Person.FirstOrDefaultAsync((Guid) user.PersonId!);
        user.Name = Input.Name;
        user.Surname = Input.Surname;
        if (person != null)
        {
            person.Name = user.Name;
            person.Surname = user.Surname;
            _uow.Person.Update(person);
            await _uow.SaveChangesAsync();
        }

        await _userManager.UpdateAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        var oldNameClaim = claims.FirstOrDefault(c => c.Type.Equals("aspnet.name"));
        var oldSurnameClaim = claims.FirstOrDefault(c => c.Type.Equals("aspnet.surname"));
        await _userManager.RemoveClaimAsync(user, oldNameClaim);
        await _userManager.RemoveClaimAsync(user, oldSurnameClaim);

        var nameClaim = new Claim("aspnet.name", user.Name);
        var surnameClaim = new Claim("aspnet.surname", user.Surname);
        await _userManager.AddClaimAsync(user, nameClaim);
        await _userManager.AddClaimAsync(user, surnameClaim);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        public string Surname { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}