using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControlInventario.Web.Api.Security
{
    public class AccessToken
    {
        public enum ClaimEnum
        {
            currentApiKey
        }

        public static string GenerateAccessToken(Guid apiKey)
        {
            List<Claim> listClaims = new List<Claim>()
            {
                new Claim(ClaimEnum.currentApiKey.ToString(), apiKey.ToString())
            };

            SymmetricSecurityKey key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(ControlInventario.Web.Api.Security.ApiKey.Secret));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            EncryptingCredentials encryptingCredentials = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
                issuer: "controlInventario",
                audience: "controlInventario",
                subject: new ClaimsIdentity(listClaims),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(8),
                issuedAt: DateTime.Now,
                signingCredentials: creds,
                encryptingCredentials: encryptingCredentials,
                claimCollection: listClaims.ToDictionary(k => k.Type, v => (object)v.Value)
                );

            string encryptedJWT = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return encryptedJWT;
        }
    }
}