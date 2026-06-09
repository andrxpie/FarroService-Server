using FarroService.BLL.Dto.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FarroService.BLL.MediatR.ApplicationUser.Login;

/// <summary>
/// Handler for executing the authentication process and issuing a secure JWT.
/// </summary>
public class LoginApplicationUserHandler : IRequestHandler<LoginApplicationUserCommand, LoginApplicationUserResponseDto?>
{
    private readonly UserManager<DAL.Entities.ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public LoginApplicationUserHandler(
        UserManager<DAL.Entities.ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<LoginApplicationUserResponseDto?> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Dto.Email);
        if (user == null)
        {
            return null;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Dto.Password);
        if (!isPasswordValid)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "Guest";

        var secret = _configuration["JwtSettings:Secret"] ?? "SuperSecretFarroServiceBookingKey2026!!!";
        var key = Encoding.ASCII.GetBytes(secret);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, primaryRole),
            new(ClaimTypes.GivenName, user.FullName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _configuration["JwtSettings:Issuer"] ?? "FarroService",
            Audience = _configuration["JwtSettings:Audience"] ?? "FarroClient",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new LoginApplicationUserResponseDto(
            tokenString,
            user.Email ?? string.Empty,
            user.FullName,
            primaryRole,
            user.Id
        );
    }
}
