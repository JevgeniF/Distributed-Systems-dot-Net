using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Contracts.Public;
using App.DAL.EF;
using App.Domain.Identity;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppUser = App.Domain.Identity.AppUser;

namespace WebApp.ApiControllers.Identity;

/// <summary>
///     Controller for login, register and user role change operations.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly ILogger<AccountController> _logger;
    private readonly IAppPublic _public;
    private readonly Random _rnd = new();
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    ///     Constructor for AccountController class
    /// </summary>
    /// <param name="signInManager">SignInManager class instance </param>
    /// <param name="userManager">UserManager class instance</param>
    /// <param name="logger">Logger interface ILogger</param>
    /// <param name="configuration">Configuration interface IConfiguration</param>
    /// <param name="appPublic">Public layer interface IAppPublic</param>
    /// <param name="context">AppDbContext class instance</param>
    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, IConfiguration configuration, IAppPublic appPublic, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        _public = appPublic;
        _context = context;
    }

    /// <summary>
    ///     Method for user logIn
    /// </summary>
    /// <param name="loginData">User data (email, password) from request body</param>
    /// <returns>JwtResponse entity, which contains JWT Token, RefreshToken and user email</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), 200)]
    [ProducesResponseType(404)]
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login loginData)
    {
        // verify username
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }

        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }

        appUser.RefreshTokens = await _context
            .Entry(appUser)
            .Collection(a => a.RefreshTokens!)
            .Query()
            .Where(t => t.AppUserId == appUser.Id)
            .ToListAsync();

        foreach (var userRefreshToken in appUser.RefreshTokens)
            if (userRefreshToken.TokenExpirationDateTime < DateTime.UtcNow &&
                userRefreshToken.PreviousTokenExpirationDateTime < DateTime.UtcNow)
                _context.RefreshTokens.Remove(userRefreshToken);

        var refreshToken = new AppRefreshToken
        {
            AppUserId = appUser.Id
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"],
            _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );

        var res = new JwtResponse
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };

        return Ok(res);
    }

    /// <summary>
    ///     Method for user registration. Registers user and post name nad surname to persons table.
    /// </summary>
    /// <param name="registrationData">User data from body: (email, password, name, surname)</param>
    /// <returns>JwtResponse entity, which contains JWT Token, RefreshToken and user email</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), 200)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> Register(Register registrationData)
    {
        //verify user
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} found", registrationData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return BadRequest("Email/Password problem");
        }

        var refreshToken = new AppRefreshToken();
        appUser = new AppUser
        {
            Name = registrationData.Name,
            Surname = registrationData.Surname,
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<AppRefreshToken>
            {
                refreshToken
            }
        };
        //create user
        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded) return BadRequest(result);
        await _userManager.AddClaimAsync(appUser, new Claim("aspnet.name", appUser.Name));
        await _userManager.AddClaimAsync(appUser, new Claim("aspnet.surname", appUser.Surname));
        await _userManager.AddToRoleAsync(appUser, "user");

        var person = await _public.Person.GetByNames(appUser.Name, appUser.Surname);
        if (person == null)
        {
            person = new Person
            {
                Id = Guid.NewGuid(),
                Name = appUser.Name,
                Surname = appUser.Surname
            };
            person = _public.Person.Add(person);
            await _public.SaveChangesAsync();
        }

        appUser.PersonId = person.Id;
        await _userManager.UpdateAsync(appUser);

        //get full user
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            _logger.LogWarning("Registration error. App User with email {} not found", registrationData.Email);
            return BadRequest("Email/Password problem");
        }

        //get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", registrationData.Email);
            return BadRequest("Email/Password problem");
        }

        //generate JWT
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims, _configuration["JWT:Key"], _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );
        var res = new JwtResponse
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }

    /// <summary>
    ///     Method for new refresh token generation
    /// </summary>
    /// <param name="refreshTokenModel">RefreshTokenModel class instance. Contains JWT and refresh token</param>
    /// <returns>JwtResponse entity, which contains new JWT Token, new RefreshToken and user email</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), 200)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
    {
        // get info from JWT
        JwtSecurityToken jwt;
        try
        {
            jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwt == null) return BadRequest("No token");
        }
        catch (Exception e)
        {
            return BadRequest($"Cant parse the token: {e.Message}");
        }

        var userEmail = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null) return BadRequest("No email");
        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);

        // compare refresh tokens
        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query().Where(t => (t.Token == refreshTokenModel.RefreshToken &&
                                 t.TokenExpirationDateTime > DateTime.UtcNow) ||
                                (t.PreviousToken == refreshTokenModel.RefreshToken &&
                                 t.PreviousTokenExpirationDateTime > DateTime.UtcNow)).ToListAsync();

        if (appUser.RefreshTokens == null) return Problem("RefreshTokens collection is null");

        if (appUser.RefreshTokens.Count == 0)
            return Problem("RefreshTokens collection is empty, no valid refresh tokens found");

        if (appUser.RefreshTokens.Count != 1) return Problem("More than one valid refresh token found");

        // generate new jwt
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", userEmail);
            return BadRequest("Email/Password problem");
        }

        //generate JWT
        var newJwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims, _configuration["JWT:Key"], _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );

        // make new refresh token
        var refreshToken = appUser.RefreshTokens.First();
        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(1);
            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.TokenExpirationDateTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var res = new JwtResponse
        {
            Token = newJwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }

    /// <summary>
    ///     For admin only. Method for changing of user role (newbie, user, admin, moderator)
    /// </summary>
    /// <param name="userAssignmentData">UserAssignment class instance, contains user email and new user role.</param>
    /// <returns>response Ok in case of success or Bad request response</returns>
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> ChangeRole([FromBody] UserAssignment userAssignmentData)
    {
        var appUser = await _userManager.FindByEmailAsync(userAssignmentData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("Assignment problem. App User with email {} not found", userAssignmentData.Email);
            return BadRequest("User problem");
        }

        var roles = await _userManager.GetRolesAsync(appUser);

        if (userAssignmentData.NewRole == "user")
        {
            var result = await _userManager.RemoveFromRolesAsync(appUser, roles);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(appUser, userAssignmentData.NewRole);
                return Ok(result);
            }
        }

        if (userAssignmentData.NewRole is "admin" or "moderator")
        {
            var result = await _userManager.RemoveFromRolesAsync(appUser, roles);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(appUser, new[] { "user", userAssignmentData.NewRole });
                return Ok(result);
            }
        }

        _logger.LogWarning("Assignment problem. Unable to change role to {}", userAssignmentData.NewRole);
        return BadRequest("Unable to change roles");
    }

    /// <summary>
    /// For Admin only. Method returns users Id's names and lists of their roles.
    /// </summary>
    /// <returns>List of objects generated from Users </returns>
    [HttpGet]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<object>> UsersList()
    {
        
        var query = _userManager.Users.AsNoTracking();
        var appUsers = await query.ToListAsync();
        var usersWithRoles = new List<object>();

        foreach (var appUser in appUsers)
        {
            var roles = await _userManager.GetRolesAsync(appUser);
            usersWithRoles.Add(new
            {
                appUser.Id,
                appUser.Name,
                appUser.Surname,
                Roles = roles
            });
        }
        return usersWithRoles;
    }
}