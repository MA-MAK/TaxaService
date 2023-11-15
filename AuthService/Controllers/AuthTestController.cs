using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;

[ApiController]
[Route("[controller]")]
public class AuthTestController : ControllerBase
{

    private readonly ILogger<AuthTestController> _logger;
    private readonly IConfiguration _config;

    public AuthTestController(ILogger<AuthTestController> logger, IConfiguration config)
    {
        _config = config;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("test")]
    public async Task<IActionResult> GetTest()
    {
        return Ok("You're authorized");
    }

}
