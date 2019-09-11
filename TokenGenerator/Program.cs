using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TokenGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Here is your token:");
            
            var securityTokenDescriptor = new SecurityTokenDescriptor();
            var keyBytes = Encoding.ASCII.GetBytes("Some secret string. Should use something like X509 certs anyway");
            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);
            securityTokenDescriptor.SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            
            const string apiUrl = "https://localhost:44319/";
            securityTokenDescriptor.Issuer = apiUrl;
            securityTokenDescriptor.Audience = apiUrl;

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = false; //make this a 'forever' token, which should only be used during development
            var tokenString = jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptor);
            
            Console.WriteLine(tokenString);
        }
    }
}
