using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Common;
using App.Domain.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO.Identity;

namespace WebApp.ApiControllers.Identity;

[Route("api/identity/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly Random _rnd = new();
    private readonly AppDbContext _context;
    private readonly IAppUOW _uow;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, IConfiguration configuration, IAppUOW uow, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
        _uow = uow;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login loginData)
    {
        //verify username
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }

        //verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }

        //get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound("User/Password problem");
        }

        //generate JWT
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims, _configuration["JWT:Key"], _configuration["JWT:Issuer"],
            _configuration["JWT:Issuer"],
            DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
        );
        var res = new JwtResponse()
        {
            Token = jwt,
            Email = appUser.Email
        };
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> Register(Register registrationData)
    {
        //verify user
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} not found", registrationData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return BadRequest("Email/Password problem");
        }

        var refreshToken = new RefreshToken();
        appUser = new AppUser()
        {
            Name = registrationData.Name,
            Surname = registrationData.Surname,
            Email = registrationData.Email,
            UserName = registrationData.Email,
            RefreshTokens = new List<RefreshToken>()
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

        var person = await _uow.Person.GetByNames(appUser.Name, appUser.Surname);
        if (person == null)
        {
            person = new Person
            {
                Id = Guid.NewGuid(),
                Name = appUser.Name,
                Surname = appUser.Surname
            };
            person = _uow.Person.Add(person);
            await _uow.SaveChangesAsync();
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
        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        // get info from JWT
        JwtSecurityToken jwt;
        try
        {
            jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenDto.JWT);
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
        if (appUser == null) return NotFound($"User with email {userEmail} not found");

        // TODO: validate token signature

        // compare refresh tokens
        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query().Where(t => (t.Token == refreshTokenDto.RefreshToken &&
                                 t.ExpirationDateTime > DateTime.UtcNow) ||
                                (t.PreviousToken == refreshTokenDto.RefreshToken &&
                                 t.PreviousExpirationDateTime > DateTime.UtcNow)).ToListAsync();

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
        if (refreshToken.Token == refreshTokenDto.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousExpirationDateTime = DateTime.UtcNow.AddMinutes(1);
            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.ExpirationDateTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var res = new JwtResponse()
        {
            Token = newJwt,
            RefreshToken = refreshToken.Token,
            Email = appUser.Email
        };
        return Ok(res);
    }
}