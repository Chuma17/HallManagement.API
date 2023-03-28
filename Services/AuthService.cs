using HallManagementTest2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HallManagementTest2.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //Student Access Token
        public string CreateStudentToken(Student student)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, student.StudentId.ToString()),
                new Claim(ClaimTypes.SerialNumber, student.MatricNo),
                new Claim(ClaimTypes.Gender, student.Gender),
                new Claim(ClaimTypes.UserData, student.HallId.ToString()),
                new Claim(ClaimTypes.Role, student.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        //Student refresh token
        public void SetStudentRefreshToken(RefreshToken newRefreshToken, Student student, HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            student.RefreshToken = newRefreshToken.Token;
            student.TokenExpires = newRefreshToken.Expires;
            student.TokenCreated = newRefreshToken.Created;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////


        //Chief Hall Admin access token
        public string CreateChiefHallAdminToken(ChiefHallAdmin chiefHallAdmin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, chiefHallAdmin.ChiefHallAdminId.ToString()),
                new Claim(ClaimTypes.Gender, chiefHallAdmin.Gender),
                new Claim(ClaimTypes.Role, chiefHallAdmin.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        //Chief Hall Admin refresh token
        public void SetCHiefHallAdminRefreshToken(RefreshToken newRefreshToken, ChiefHallAdmin chiefHallAdmin, HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            chiefHallAdmin.RefreshToken = newRefreshToken.Token;
            chiefHallAdmin.TokenExpires = newRefreshToken.Expires;
            chiefHallAdmin.TokenCreated = newRefreshToken.Created;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////


        //Hall Admin token
        public string CreateHallAdminToken(HallAdmin hallAdmin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, hallAdmin.HallAdminId.ToString()),
                new Claim(ClaimTypes.Role, hallAdmin.Role),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        //Hall Admin refresh token
        public void SetHallAdminRefreshToken(RefreshToken newRefreshToken, HallAdmin hallAdmin, HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            hallAdmin.RefreshToken = newRefreshToken.Token;
            hallAdmin.TokenExpires = newRefreshToken.Expires;
            hallAdmin.TokenCreated = newRefreshToken.Created;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////


        //Porter token        
        public string CreatePorterToken(Porter porter)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, porter.PorterId.ToString()),
                new Claim(ClaimTypes.UserData, porter.HallId.ToString()),
                new Claim(ClaimTypes.Role, porter.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        //Porter refresh token
        public void SetPorterRefreshToken(RefreshToken newRefreshToken, Porter porter, HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            porter.RefreshToken = newRefreshToken.Token;
            porter.TokenExpires = newRefreshToken.Expires;
            porter.TokenCreated = newRefreshToken.Created;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////


        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(3),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
