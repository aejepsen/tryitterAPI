using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tryitter.Models;
using Microsoft.IdentityModel.Tokens;
using tryitter.Constants;

namespace tryitter.Services
{
    public class TokenGenerator
  {
    public string Generate(Student student)
    {
      return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor()
      {
        Subject = AddClaims(student),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenConstants.Secret)), SecurityAlgorithms.HmacSha256Signature),
        Expires = DateTime.Now.AddDays(1)
      }));
    }
    private static ClaimsIdentity AddClaims(Student student)
    {
      var claims = new ClaimsIdentity();
      claims.AddClaim(new Claim(ClaimTypes.Sid, student.StudentId.ToString()));
      claims.AddClaim(new Claim(ClaimTypes.Name, student.Name));
      claims.AddClaim(new Claim(ClaimTypes.Email, student.Email));
      return claims;
    }
  }
}