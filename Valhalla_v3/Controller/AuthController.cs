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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(UserManager<Operator> userManager, SignInManager<Operator> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("41b93ca9702dda36aeb5ba9a0734c0627e6de25255f7d766d6a278a16c98fd75833c19b2b54f880d74a5594efe6f3f4171c58b82efb746d2cd838092d36fcca042beb410162f21709cfc1408787f2642651be12ce8e91fccc59930377550712cba105cf9439cbcaa3c0971d94c462c2f9b3b479330a30f052ad90ac538c1cb9851dac581a1350a15b3ec047faf4b83bc058884232067fdd6b9775f52dcd5db9964c3f804e62107cc7182255f918b6defa3778494cd761eeb99cd6b23a08cc8f9d5d96737516901aad069f14d7d5fe0e9a1a00bac9027eaf9441e961be7a5c1ec7f0241ae5f4e98c47bda73420cfcb8b2f6e19ce723ab423d4dc9b40547f57c2b"));

                var token = new JwtSecurityToken(
                    issuer: "Valhallav3",
                    audience: "Valhallav3",
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                };

                _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthCookie", new JwtSecurityTokenHandler().WriteToken(token), cookieOptions);
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
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string token)
    {
        try
        {
            var cookieToken = _httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Nie znaleziono tokena w ciasteczku 'AuthCookie'.");
            
            if (cookieToken != token)
                return Unauthorized("Przesłany token jest niezgodny");

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var user = await _userManager.FindByNameAsync(userName);
            
            if (user == null)
                return Unauthorized("Błędny użytkownik przekazany w tokenie");

            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AuthCookie");

            return Ok(new { message = "Wylogowano pomyślnie." });
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
}
