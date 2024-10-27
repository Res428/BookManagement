using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class JwtToken
{
    string keyValue = "anthing_api_key_you_want_anthing_api_key_you_want_anthing_api_key_you_want_anthing_api_key_you_want";
    string issuerValue = "http://localhost";
    string audienceValue = "bookmanagement api";

    // public string GenerateJwtToken(string? username, string? role)
    // {
    //     try
    //     {
    //         var keyBytes = System.Text.Encoding.UTF8.GetBytes(keyValue);

    //         // Kiểm tra xem key có đủ dài không
    //         if (keyBytes.Length < 40)
    //         {
    //             throw new Exception("Key is not supported. Please check again");
    //         }

    //         var securityKey = new SymmetricSecurityKey(keyBytes);
    //         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

    //         // Thêm vai trò vào claims
    //         var claims = new[] {
    //             new Claim("UserName", username ?? ""),
    //             // new Claim("Role", role ?? "")
    //             // new Claim(ClaimTypes.Role, "admin") 
    //             new Claim(ClaimTypes.Role, role ?? "")
    //         };

    //         var token = new JwtSecurityToken(
    //             issuer: issuerValue,
    //             audience: audienceValue,
    //             claims: claims,
    //             expires: DateTime.UtcNow.AddDays(1),
    //             signingCredentials: credentials
    //         );

    //         return new JwtSecurityTokenHandler().WriteToken(token);
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception(ex.Message);
    //     }
    // }

    // public string GenerateJwtToken(string? username, int userId, string? role)
    // {
    //     try
    //     {
    //         var keyBytes = System.Text.Encoding.UTF8.GetBytes(keyValue);

    //         if (keyBytes.Length < 40)
    //         {
    //             throw new Exception("Key is not supported. Please check again");
    //         }

    //         var securityKey = new SymmetricSecurityKey(keyBytes);
    //         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

    //         var claims = new[] {
    //         new Claim("UserName", username ?? ""),
    //         new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Thêm ID người dùng
    //         new Claim(ClaimTypes.Role, role ?? "")
    //     };

    //         var token = new JwtSecurityToken(
    //             issuer: issuerValue,
    //             audience: audienceValue,
    //             claims: claims,
    //             expires: DateTime.UtcNow.AddDays(1),
    //             signingCredentials: credentials
    //         );

    //         return new JwtSecurityTokenHandler().WriteToken(token);
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception(ex.Message);
    //     }
    // }

    public string GenerateJwtToken(string? username, int userId, string? role)
    {
        try
        {
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(keyValue);

            if (keyBytes.Length < 40)
            {
                throw new Exception("Key is not supported. Please check again");
            }

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            // Sử dụng tham số userId và username
            var claims = new[] {
            new Claim(ClaimTypes.Name, username ?? ""),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role ?? ""),
            new Claim("id", userId.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: issuerValue,
                audience: audienceValue,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    // public void CreateUserClaim(int userId)
    // {
    //     // Convert userId to string before creating the claim
    //     var claim = new Claim(ClaimTypes.NameIdentifier, userId.ToString());

    //     // Proceed with adding the claim to the user or whatever your logic is
    // }


    // public bool ValidateToken(string token, out ClaimsPrincipal? claims)
    // {
    //     claims = null;
    //     var tokenHandler = new JwtSecurityTokenHandler();

    //     var tokenValidationParameters = new TokenValidationParameters
    //     {
    //         ValidateIssuer = true,
    //         ValidateAudience = true,
    //         ValidateLifetime = true,
    //         ValidateIssuerSigningKey = true,
    //         ValidIssuer = issuerValue,
    //         ValidAudience = audienceValue,
    //         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyValue))
    //     };

    //     try
    //     {
    //         claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
    //         return true;
    //     }
    //     catch (SecurityTokenException)
    //     {
    //         return false;
    //     }
    // }

    public bool ValidateToken(string token, out ClaimsPrincipal? claims)
    {
        claims = null;
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuerValue,
            ValidAudience = audienceValue,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyValue))
        };

        try
        {
            claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return true;
        }
        catch (SecurityTokenExpiredException)
        {
            // Token đã hết hạn
            return false;
        }
        catch (SecurityTokenException)
        {
            // Token không hợp lệ
            return false;
        }
        catch (Exception)
        {
            // Xử lý các lỗi khác
            return false;
        }
    }


    internal object GenerateJwtToken(string? username)
    {
        throw new NotImplementedException();
    }
}
