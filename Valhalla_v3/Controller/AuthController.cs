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
        try
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

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("41b93ca9702dda36aeb5ba9a0734c0627e6de25255f7d766d6a278a16c98fd75833c19b2b54f880d74a5594efe6f3f4171c58b82efb746d2cd838092d36fcca042beb410162f21709cfc1408787f2642651be12ce8e91fccc59930377550712cba105cf9439cbcaa3c0971d94c462c2f9b3b479330a30f052ad90ac538c1cb9851dac581a1350a15b3ec047faf4b83bc058884232067fdd6b9775f52dcd5db9964c3f804e62107cc7182255f918b6defa3778494cd761eeb99cd6b23a08cc8f9d5d96737516901aad069f14d7d5fe0e9a1a00bac9027eaf9441e961be7a5c1ec7f0241ae5f4e98c47bda73420cfcb8b2f6e19ce723ab423d4dc9b40547f57c2b"));

                var token = new JwtSecurityToken(
                    issuer: "Valhallav3",
                    audience: "Valhallav3",
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
        catch(Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
}
