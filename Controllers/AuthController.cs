using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PositivePOSAPI.Data;
using PositivePOSAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PositivePOSAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _config;
    private readonly PositiveDbContext _positiveDb;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration config,
        PositiveDbContext positiveDb)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _positiveDb = positiveDb;
    }

    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest? req)
    {
        if (req is null)
            return BadRequest("Send JSON: { \"email\": \"...\", \"password\": \"...\" }");

        var email = req.Email?.Trim();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Email and password are required.");

        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
            return BadRequest("Email already exists.");

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            NormalizedUserName = email.ToUpperInvariant(),
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new
        {
            ok = true,
            userId = user.Id,
            email = user.Email
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest? req)
    {
        if (req is null)
            return BadRequest("Send JSON: { \"email\": \"...\", \"password\": \"...\" }");

        var email = req.Email?.Trim();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Email and password are required.");

        // 1) Identity lookup only -> AspNetUsers
        var user = await _userManager.FindByEmailAsync(email);

        // Optional fallback: allow username in the email box
        if (user is null)
            user = await _userManager.FindByNameAsync(email);

        if (user is null)
            return Unauthorized("Invalid credentials.");

        // 2) Identity password verification only -> AspNetUsers.PasswordHash
        var signInResult = await _signInManager.CheckPasswordSignInAsync(
            user,
            req.Password,
            lockoutOnFailure: true);

        if (!signInResult.Succeeded)
            return Unauthorized("Invalid credentials.");

        // 3) Identity roles only -> AspNetRoles/AspNetUserRoles
        var roles = await _userManager.GetRolesAsync(user);

        // 4) App-specific cascading can happen AFTER auth succeeds
        // Add your own user/company/location/profile lookup here later.
        // Example:
        // var profile = await _positiveDb.UserProfiles.FirstOrDefaultAsync(x => x.AspNetUserId == user.Id);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? email),
            new(ClaimTypes.Name, user.UserName ?? email),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(key))
            return StatusCode(500, "Jwt:Key missing in configuration.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiresUtc = DateTime.UtcNow.AddHours(8);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiresUtc,
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiresUtc,
            userId = user.Id,
            email = user.Email,
            userName = user.UserName,
            roles = roles.ToArray()
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email),
            userName = User.FindFirstValue(ClaimTypes.Name),
            roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray()
        });
    }
}