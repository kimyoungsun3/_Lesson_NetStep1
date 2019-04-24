using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.IdentityModel.Tokens.Jwt;


namespace UpbitAPI
{
	class Program
	{
		static void Main(string[] args)
		{
		}

		void UpbitAPI()
		{

			//TimeSpan diff = DateTime.Now - new DateTime(1970, 1, 1);
			//var nonce = Convert.ToInt64(diff.TotalMilliseconds);
			//var payload = new JwtPayload
			//{
			//	{ "access_key", "발급받은 Access Key" },
			//	{ "nonce", nonce }
			//};
			//byte[] keyBytes = Encoding.Default.GetBytes("발급받은 Secret Key");
			//var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes);
			//var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, "HS256");
			//var header = new JwtHeader(credentials);
			//var secToken = new JwtSecurityToken(header, payload);

			//var jwtToken = new JwtSecurityTokenHandler().WriteToken(secToken);
			//var authorizationToken = "Bearer " + jwtToken;

			//Console.WriteLine(authorizationToken);
		}
	}
}
