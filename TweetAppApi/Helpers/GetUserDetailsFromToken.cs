using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace TweetAppApi.Helpers
{
    public class GetUserDetailsFromToken
    {
        public string GetEmail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
             var d = decodedValue.Claims.ToList();
            string email = d[0].Value.ToString();
            return email;
        }

        public string GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var d = decodedValue.Claims.ToList();
            string id = d[1].Value.ToString();
            return id;
        }
        public string GetUsername(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var d = decodedValue.Claims.ToList();
            string username = d[2].Value.ToString();
            return username;
        }


    }
}
