using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Valhalla_v3.Shared;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<Operator> _userManager;
    private readonly SignInManager<Operator> _signInManager;

    public AuthController(UserManager<Operator> userManager, SignInManager<Operator> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User") // Dostosuj do roli użytkownika
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey"));

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }
}
