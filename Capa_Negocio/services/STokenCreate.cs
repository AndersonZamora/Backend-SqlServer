using Capa_Entidada;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Capa_Negocio
{
    public class STokenCreate : ITokenCreate
    {
        public ClaimsIdent ValidarToken(string token, string code, ClaimsIdentity identity)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.UTF8.GetBytes(code);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                ClaimsIdent user = GetUser(identity);

                string newToken = TokenCreate(user.Id_Usuario, user.Name, code);
                user.Token = newToken;

                return user;
            }
            catch
            {
                return null;
            }
        }

        public ClaimsIdent GetUser(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var claim = identity.Claims;
                
                string id = claim.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                string no = claim.FirstOrDefault(n => n.Type == ClaimTypes.Surname)?.Value;

                ClaimsIdent user = new ClaimsIdent()
                {
                    Id_Usuario = int.Parse(id),
                    Name = no
                };

                return user;
            }

            return null;
        }

        public string TokenCreate(int id, string nombre, string code)
        {
            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(code);
                var clains = new ClaimsIdentity();
               
                clains.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
                clains.AddClaim(new Claim(ClaimTypes.Surname, nombre));
                
                var tokenDescr = new SecurityTokenDescriptor
                {
                    Subject = clains,
                    Expires = DateTime.UtcNow.AddMinutes(120),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHan = new JwtSecurityTokenHandler();
                var tokenC = tokenHan.CreateToken(tokenDescr);
                string token = tokenHan.WriteToken(tokenC);
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
