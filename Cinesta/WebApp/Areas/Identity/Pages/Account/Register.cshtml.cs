// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable CS1591
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using App.Contracts.DAL;
using App.DAL.DTO;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<AppUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IAppUOW _uow;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserStore<AppUser> _userStore;

    public RegisterModel(
        IAppUOW uow,
        UserManager<AppUser> userManager,
        IUserStore<AppUser> userStore,
        SignInManager<AppUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        _uow = uow;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; }


    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            user.Name = Input.Name;
            user.Surname = Input.Surname;

            var result = await _userManager.CreateAsync(user, Input.Password);


            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                result = await _userManager.AddClaimAsync(user, new Claim("aspnet.name", user.Name));
                result = await _userManager.AddClaimAsync(user, new Claim("aspnet.surname", user.Surname));
                result = await _userManager.AddToRoleAsync(user, "user");

                var person = await _uow.Person.GetByNames(user.Name, user.Surname);
                if (person == null)
                {
                    person = new Person
                    {
                        Id = Guid.NewGuid(),
                        Name = user.Name,
                        Surname = user.Surname
                    };
                    person = _uow.Person.Add(person);
                    await _uow.SaveChangesAsync();
                }

                user.PersonId = person.Id;
                result = await _userManager.UpdateAsync(user);

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    null,
                    new { area = "Identity", userId, code, returnUrl },
                    Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, Base.Resources.Identity.confirmYourEmail,
                    $"{Base.Resources.Identity.confirmEmailText} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{Base.Resources.Identity.clickingHere}</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });

                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private AppUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<AppUser>();
        }
        catch
        {
            throw new InvalidOperationException($"{Base.Resources.Identity.cantCreateInstance} '{nameof(AppUser)}'.");
        }
    }

    private IUserEmailStore<AppUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException(Base.Resources.Identity.uiRequiresUserStoreWithEmail);
        return (IUserEmailStore<AppUser>)_userStore;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Required]
        [StringLength(25, ErrorMessageResourceName = "lengthErrorMessage",
            ErrorMessageResourceType = typeof(Base.Resources.Identity), MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(25, ErrorMessageResourceName = "lengthErrorMessage",
            ErrorMessageResourceType = typeof(Base.Resources.Identity), MinimumLength = 1)]
        public string Surname { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessageResourceName = "lengthErrorMessage",
            ErrorMessageResourceType = typeof(Base.Resources.Identity), MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceName = "passwrdDoesntMatchError",
            ErrorMessageResourceType = typeof(Base.Resources.Identity))]
        public string ConfirmPassword { get; set; }
    }
}