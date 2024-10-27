using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace jwt_demo.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly JwtToken _jwt;

    public BookController(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwt = new JwtToken();
    }

    // Phương thức GET lấy danh sách mượn sách hoặc thông tin mượn sách theo mã
    [HttpGet(Name = "Get_Muon_Sach")]
    public async Task<ActionResult> GetMuonSach([FromQuery] int? muonSachId)
    {
        try
        {
            string authHeader = "";
            string token = "";

            // Lấy token từ header Authorization
            authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header" });
            }

            token = authHeader["Bearer ".Length..].Trim();

            // Gọi hàm ValidateToken
            if (_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
            {
                var userName = claims?.FindFirst(c => c.Type == "UserName")?.Value;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                using SqlCommand command = new();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;

                // Nếu có truyền mã mượn sách, tìm thông tin theo ID
                if (muonSachId.HasValue)
                {
                    command.CommandText = "spMuonSach_SelectById";
                    command.Parameters.AddWithValue("@MuonSachID", muonSachId);
                }
                else
                {
                    command.CommandText = "spMuonSach_SelectALL";
                }

                SqlDataAdapter da = new(command);
                DataTable dt = new();
                da.Fill(dt);

                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { data = dt }),
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            else
            {
                return Unauthorized(new { message = "Token is invalid" });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message.ToString() });
        }
    }

    // Phương thức POST để thêm hoặc chỉnh sửa thông tin mượn sách
    [HttpPost("update")]
    public async Task<ActionResult> UpdateMuonSach()
    {
        try
        {
            string authHeader = "";
            string token = "";

            // Lấy token từ header Authorization
            authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header" });
            }

            token = authHeader["Bearer ".Length..].Trim();

            // Gọi hàm ValidateToken
            if (_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
            {
                var userName = claims?.FindFirst(c => c.Type == "UserName")?.Value;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                using SqlCommand command = new();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spMuonSach_Update";

                // Lấy thông tin mượn sách từ body
                using (var reader = new StreamReader(Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    var jsonObject = JObject.Parse(requestBody);

                    command.Parameters.AddWithValue("@MuonSachID", jsonObject["MuonSachID"]?.Value<int>());
                    command.Parameters.AddWithValue("@MaSach", jsonObject["MaSach"]?.Value<int>());
                    command.Parameters.AddWithValue("@NgayMuon", jsonObject["NgayMuon"]?.Value<DateTime>());
                    command.Parameters.AddWithValue("@NgayTra", jsonObject["NgayTra"]?.Value<DateTime>());
                    command.Parameters.AddWithValue("@UserName", userName);
                }

                SqlDataAdapter da = new(command);
                DataTable dt = new();
                da.Fill(dt);

                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { message = dt.Rows[0]["errMsg"] }),
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            else
            {
                return Unauthorized(new { message = "Token is invalid" });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message.ToString() });
        }
    }

    // Phương thức POST để xóa thông tin mượn sách
    [HttpPost("delete")]
    public async Task<ActionResult> DeleteMuonSach()
    {
        try
        {
            string authHeader = "";
            string token = "";

            // Lấy token từ header Authorization
            authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header" });
            }

            token = authHeader["Bearer ".Length..].Trim();

            // Gọi hàm ValidateToken
            if (_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
            {
                var userName = claims?.FindFirst(c => c.Type == "UserName")?.Value;

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    await connection.OpenAsync();
                }

                using SqlCommand command = new();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spMuonSach_Delete";

                // Lấy thông tin mượn sách từ body
                using (var reader = new StreamReader(Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    var jsonObject = JObject.Parse(requestBody);

                    command.Parameters.AddWithValue("@MuonSachID", jsonObject["MuonSachID"]?.Value<int>());
                    command.Parameters.AddWithValue("@UserName", userName);
                }

                SqlDataAdapter da = new(command);
                DataTable dt = new();
                da.Fill(dt);

                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { message = dt.Rows[0]["errMsg"] }),
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
            else
            {
                return Unauthorized(new { message = "Token is invalid" });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message.ToString() });
        }
    }
}
