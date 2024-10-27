using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
// using Newtonsoft.Json.Linq;
// using System.ComponentModel.DataAnnotations;

namespace jwt_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class RegisterModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
        }


        // public class ResetPasswordModel
        // {
        //     public string NewPassword { get; set; }
        // }

        // [AllowAnonymous]
        // [HttpPost("login")]
        // public async Task<IActionResult> UserCheckAuthentication([FromBody] LoginModel loginModel)
        // {
        //     if (loginModel == null || string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
        //     {
        //         return BadRequest(new { message = "Please input Username and Password!" });
        //     }

        //     try
        //     {
        //         string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //         using var connection = new SqlConnection(connectionString);
        //         await connection.OpenAsync();

        //         using var command = new SqlCommand("UserLogin", connection)
        //         {
        //             CommandType = CommandType.StoredProcedure
        //         };

        //         command.Parameters.AddWithValue("@UserName", loginModel.UserName);
        //         command.Parameters.AddWithValue("@PlainPassword", loginModel.Password);

        //         using var da = new SqlDataAdapter(command);
        //         var dt = new DataTable();
        //         da.Fill(dt);

        //         if (dt.Rows.Count == 0)
        //         {
        //             return Unauthorized(new { message = "Invalid Username or Password" });
        //         }

        //         var userRole = dt.Rows[0]["Role"].ToString();
        //         var jwt = new JwtToken();
        //         var tokenString = jwt.GenerateJwtToken(username: dt.Rows[0]["UserName"].ToString(), role: userRole);

        //         return Ok(new { token = tokenString });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception (consider using a logging framework)
        //         return StatusCode(500, new { message = "An error occurred while processing your request." });
        //     }
        // }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> UserCheckAuthentication([FromBody] LoginModel loginModel)
        {
            try
            {
                if (loginModel == null || string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
                {
                    return BadRequest(new { message = "Please input Username and Password!" });
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("UserLogin", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserName", loginModel.UserName);
                command.Parameters.AddWithValue("@PlainPassword", loginModel.Password);

                using var da = new SqlDataAdapter(command);
                var dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return Unauthorized(new { message = "Invalid Username or Password" });
                }

                var userRole = dt.Rows[0]["Role"].ToString();
                var userId = Convert.ToInt32(dt.Rows[0]["UserID"]); // Lấy userId từ DataTable
                var jwt = new JwtToken();
                var tokenString = jwt.GenerateJwtToken(username: dt.Rows[0]["UserName"].ToString(), userId: userId, role: userRole);

                return Ok(new { token = tokenString });
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }


        //{ REGISTER }

        // [AllowAnonymous]
        // [HttpPost("register")]
        // public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        // {
        //     if (registerModel == null ||
        //         string.IsNullOrEmpty(registerModel.UserName) ||
        //         string.IsNullOrEmpty(registerModel.Password) ||
        //         string.IsNullOrEmpty(registerModel.Email) ||
        //         string.IsNullOrEmpty(registerModel.FullName))
        //     {
        //         return BadRequest(new { message = "All fields are required." });
        //     }

        //     try
        //     {

        //         string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //         using var connection = new SqlConnection(connectionString);
        //         await connection.OpenAsync();

        //         using var command = new SqlCommand("RegisterUser", connection)
        //         {
        //             CommandType = CommandType.StoredProcedure
        //         };

        //         command.Parameters.AddWithValue("@Username", registerModel.UserName);
        //         command.Parameters.AddWithValue("@PlainPassword", registerModel.Password);
        //         command.Parameters.AddWithValue("@FullName", registerModel.FullName);
        //         command.Parameters.AddWithValue("@Email", registerModel.Email);



        //         await connection.OpenAsync();
        //         await command.ExecuteNonQueryAsync();

        //         return Ok(new { message = "User registered successfully." });
        //     }
        //     catch (SqlException ex)
        //     {
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null ||
                string.IsNullOrEmpty(registerModel.UserName) ||
                string.IsNullOrEmpty(registerModel.Password) ||
                string.IsNullOrEmpty(registerModel.Email) ||
                string.IsNullOrEmpty(registerModel.FullName))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Kiểm tra xem tên người dùng đã tồn tại chưa
                using var checkUserCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username OR Email = @Email", connection);
                checkUserCommand.Parameters.AddWithValue("@Username", registerModel.UserName);
                checkUserCommand.Parameters.AddWithValue("@Email", registerModel.Email);

                int userExists = (int)await checkUserCommand.ExecuteScalarAsync();
                if (userExists > 0)
                {
                    return BadRequest(new { message = "Username or email already exists." });
                }

                using var command = new SqlCommand("RegisterUser", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Username", registerModel.UserName);
                command.Parameters.AddWithValue("@FullName", registerModel.FullName);
                command.Parameters.AddWithValue("@Email", registerModel.Email);
                command.Parameters.AddWithValue("@PlainPassword", registerModel.Password);

                await command.ExecuteNonQueryAsync();

                return Ok(new { message = "User registered successfully." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // [Authorize]
        // [HttpGet("users")]
        // public async Task<IActionResult> GetUserDetails()
        // {
        //     try
        //     {
        //         var userName = User.Identity.Name;

        //         if (string.IsNullOrEmpty(userName))
        //         {
        //             return BadRequest(new { message = "User not authenticated." });
        //         }

        //         string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //         using var connection = new SqlConnection(connectionString);
        //         await connection.OpenAsync();

        //         using var command = new SqlCommand("GetUserDetails", connection)
        //         {
        //             CommandType = CommandType.StoredProcedure
        //         };

        //         command.Parameters.AddWithValue("@UserName", userName);

        //         using var da = new SqlDataAdapter(command);
        //         var dt = new DataTable();
        //         da.Fill(dt);

        //         if (dt.Rows.Count == 0)
        //         {
        //             return NotFound(new { message = "User not found." });
        //         }

        //         return Ok(new { user = new { UserName = dt.Rows[0]["UserName"], Email = dt.Rows[0]["Email"] } });
        //     }
        //     catch (SqlException sqlEx)
        //     {
        //         // Log SQL exception
        //         return StatusCode(500, new { message = "A database error occurred.", details = sqlEx.Message });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log general exception
        //         return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        //     }
        // }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("SELECT * FROM Users", connection);
                using var reader = await command.ExecuteReaderAsync();

                var users = new List<object>();
                while (await reader.ReadAsync())
                {
                    users.Add(new
                    {
                        UserID = reader["UserID"],
                        UserName = reader["Username"],
                        FullName = reader["FullName"],
                        Email = reader["Email"],
                        PasswordHash = reader["PasswordHash"],
                        Role = reader["Role"]
                    });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }


        // [Authorize]
        // [HttpPost("reset-password")]
        // public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel passwordData)
        // {
        //     if (passwordData == null || string.IsNullOrEmpty(passwordData.NewPassword))
        //     {
        //         return BadRequest(new { message = "Please provide a new password." });
        //     }

        //     try
        //     {
        //         var userName = User.Identity.Name;
        //         string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //         using var connection = new SqlConnection(connectionString);
        //         await connection.OpenAsync();

        //         using var command = new SqlCommand("ResetUserPassword", connection)
        //         {
        //             CommandType = CommandType.StoredProcedure
        //         };

        //         // Hash the new password before updating it
        //         var hashedNewPassword = HashPassword(passwordData.NewPassword);

        //         command.Parameters.AddWithValue("@UserName", userName);
        //         command.Parameters.AddWithValue("@NewPassword", hashedNewPassword);

        //         await command.ExecuteNonQueryAsync();

        //         return Ok(new { message = "Password reset successfully." });
        //     }
        //     catch (SqlException sqlEx)
        //     {
        //         // Log SQL exception
        //         return StatusCode(500, new { message = "A database error occurred.", details = sqlEx.Message });
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception
        //         return StatusCode(500, new { message = "An error occurred while processing your request." });
        //     }
        // }

        // Method to hash passwords
        // private string HashPassword(string password)
        // {
        //     return BCrypt.Net.BCrypt.HashPassword(password);
        // }
    }
}
