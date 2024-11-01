// BookController
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace jwt_demo.Controllers;

[ApiController]
[Route("[controller]")] // Updated route here
public class BookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly JwtToken _jwt;

    public BookController(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwt = new JwtToken();
    }

    public class Book
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        // Thêm các thuộc tính khác
    }

    [AllowAnonymous]
    // GET: /books
    [HttpGet(Name = "Get_Books")]
    public async Task<ActionResult> GetBooks()
    {
        try
        {
            string authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header" });
            }

            string token = authHeader["Bearer ".Length..].Trim();
            if (!_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
            {
                return Unauthorized(new { message = "Token is invalid" });
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new()
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "SELECT * FROM Books"
            };

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            var books = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var book = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    book[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                books.Add(book);
            }

            return Ok(new { data = books });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    // {
    //     try
    //     {
    //         using var connection = new SqlConnection(_connectionString);
    //         await connection.OpenAsync();

    //         var command = new SqlCommand("SELECT * FROM Books", connection);
    //         using var reader = await command.ExecuteReaderAsync();

    //         var books = new List<Book>();
    //         while (await reader.ReadAsync())
    //         {
    //             var book = new Book
    //             {
    //                 // Gán các thuộc tính từ reader
    //                 var book = new Dictionary<string, object>();
    //                 for (int i = 0; i < reader.FieldCount; i++)
    //                 {
    //                     book[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
    //                 }
    //                 books.Add(book);
    //             };
    //             books.Add(book);
    //         }

    //         return Ok(books);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new { message = ex.Message });
    //     }
    // }


    [AllowAnonymous]
    // POST: /books/add
    [HttpPost("add")]
    public async Task<ActionResult> AddBook()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var bookData = JObject.Parse(requestBody);

            // Kiểm tra các trường bắt buộc
            if (bookData["Title"] == null || bookData["ISBN"] == null || bookData["Price"] == null)
            {
                return BadRequest(new { message = "Title, ISBN, and Price are required." });
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new()
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "AddBook"
            };

            // Thêm các tham số cho thủ tục
            command.Parameters.AddWithValue("@Title", bookData["Title"].Value<string>());
            command.Parameters.AddWithValue("@Author", bookData["Author"]?.Value<string>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Description", bookData["Description"]?.Value<string>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ISBN", bookData["ISBN"].Value<string>());
            command.Parameters.AddWithValue("@PublishedDate", bookData["PublishedDate"]?.Value<DateTime?>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Price", bookData["Price"].Value<decimal>());
            command.Parameters.AddWithValue("@StockQuantity", bookData["StockQuantity"]?.Value<int>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsAvailableForRent", bookData["IsAvailableForRent"]?.Value<bool>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@RentPrice", bookData["RentPrice"]?.Value<decimal?>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CategoryID", bookData["CategoryID"]?.Value<int>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CoverImage", bookData["CoverImage"]?.Value<string>() ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Book added successfully." });
        }
        catch (Exception ex)
        {
            // Ghi lại lỗi để phân tích
            Console.WriteLine($"Error: {ex.Message}, StackTrace: {ex.StackTrace}");
            return BadRequest(new { message = ex.Message });
        }
    }


    // [HttpPost("add")]
    // public async Task<ActionResult> AddBook()
    // {
    //     try
    //     {
    // string authHeader = Request.Headers["Authorization"];
    // if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
    // {
    //     return Unauthorized(new { message = "Missing or invalid Authorization header" });
    // }

    // string token = authHeader["Bearer ".Length..].Trim();
    // if (!_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
    // {
    //     return Unauthorized(new { message = "Token is invalid" });
    // }

    // // Kiểm tra xem claims có null không
    // if (claims == null)
    // {
    //     return Unauthorized(new { message = "Claims are null" });
    // }

    // Ghi lại tất cả claims để kiểm tra
    // foreach (var claim in claims.Claims)
    // {
    //     Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
    // }

    // var userRole = claims.FindFirst(c => c.Type == "Role")?.Value;
    // Console.WriteLine($"User Role: {userRole}"); // Ghi lại vai trò người dùng

    // Kiểm tra xem người dùng có phải là quản trị viên không
    // if (userRole != "admin")
    // {
    //     return Unauthorized(new { message = "You do not have permission to perform this action." });
    // }

    // var userName = claims.FindFirst(c => c.Type == "UserName")?.Value;

    // using var reader = new StreamReader(Request.Body);
    // var requestBody = await reader.ReadToEndAsync();
    // var bookData = JObject.Parse(requestBody);

    // // Kiểm tra các trường bắt buộc
    // if (bookData["Title"] == null || bookData["ISBN"] == null || bookData["Price"] == null)
    // {
    //     return BadRequest(new { message = "Title, ISBN, and Price are required." });
    // }

    //         string connectionString = _configuration.GetConnectionString("DefaultConnection");
    //         using SqlConnection connection = new(connectionString);
    //         await connection.OpenAsync();

    //         using SqlCommand command = new()
    //         {
    //             Connection = connection,
    //             CommandType = CommandType.StoredProcedure,
    //             CommandText = "AddBook"
    //         };

    //         // Thêm các tham số cho thủ tục
    //         command.Parameters.AddWithValue("@Title", bookData["Title"].Value<string>());
    //         command.Parameters.AddWithValue("@Author", bookData["Author"]?.Value<string>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@Description", bookData["Description"]?.Value<string>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@ISBN", bookData["ISBN"].Value<string>());
    //         command.Parameters.AddWithValue("@PublishedDate", bookData["PublishedDate"]?.Value<DateTime?>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@Price", bookData["Price"].Value<decimal>());
    //         command.Parameters.AddWithValue("@StockQuantity", bookData["StockQuantity"]?.Value<int>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@IsAvailableForRent", bookData["IsAvailableForRent"]?.Value<bool>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@RentPrice", bookData["RentPrice"]?.Value<decimal?>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@CategoryID", bookData["CategoryID"]?.Value<int>() ?? (object)DBNull.Value);
    //         command.Parameters.AddWithValue("@CoverImage", bookData["CoverImage"]?.Value<string>() ?? (object)DBNull.Value);

    //         await command.ExecuteNonQueryAsync();

    //         return Ok(new { message = "Book added successfully." });
    //     }
    //     catch (Exception ex)
    //     {
    //         // Ghi lại lỗi để phân tích
    //         Console.WriteLine($"Error: {ex.Message}, StackTrace: {ex.StackTrace}");
    //         return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
    //     }
    // }

    // [HttpPost("add")]
    // public async Task<ActionResult> AddBook([FromBody] Book book)
    // {
    //     if (book == null || string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.ISBN))
    //     {
    //         return BadRequest(new { message = "Missing required fields" });
    //     }

    //     try
    //     {
    //         using var connection = new SqlConnection(_connectionString);
    //         await connection.OpenAsync();

    //         var command = new SqlCommand("AddBookByAdmin", connection)
    //         {
    //             CommandType = CommandType.StoredProcedure
    //         };
    //         // Thêm các tham số cho thủ tục

    //         await command.ExecuteNonQueryAsync();
    //         return Ok(new { message = "Book added successfully." });
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new { message = ex.Message });
    //     }
    // }


    [AllowAnonymous]
    // PUT: /books/update/{id}
    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateBook(int id)
    {
        try
        {
            string authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header" });
            }

            string token = authHeader["Bearer ".Length..].Trim();
            if (!_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
            {
                return Unauthorized(new { message = "Token is invalid" });
            }

            var userRole = claims?.FindFirst(c => c.Type == "Role")?.Value;

            // Kiểm tra xem người dùng có phải là quản trị viên không
            if (userRole != "admin")
            {
                return Unauthorized(new { message = "You do not have permission to perform this action." });
            }

            var userName = claims?.FindFirst(c => c.Type == "UserName")?.Value;

            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var bookData = JObject.Parse(requestBody);

            // Kiểm tra các trường bắt buộc
            if (bookData["Title"] == null || bookData["Price"] == null)
            {
                return BadRequest(new { message = "Title and Price are required." });
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new()
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "UpdateBookByAdmin"
            };

            // Thêm tham số cho thủ tục
            command.Parameters.AddWithValue("@BookID", id);
            command.Parameters.AddWithValue("@Title", bookData["Title"]?.Value<string>());
            command.Parameters.AddWithValue("@Author", bookData["Author"]?.Value<string>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Description", bookData["Description"]?.Value<string>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Price", bookData["Price"]?.Value<decimal>());
            command.Parameters.AddWithValue("@StockQuantity", bookData["StockQuantity"]?.Value<int>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsAvailableForRent", bookData["IsAvailableForRent"]?.Value<bool>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@RentPrice", bookData["RentPrice"]?.Value<decimal?>() ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CoverImage", bookData["CoverImage"]?.Value<string>() ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Book updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // [HttpPut("update/{id}")]
    // public async Task<ActionResult> UpdateBook(int id, [FromBody] Book book)
    // {
    //     if (book == null || string.IsNullOrEmpty(book.Title))
    //     {
    //         return BadRequest(new { message = "Title is required." });
    //     }

    //     try
    //     {
    //         using var connection = new SqlConnection(_connectionString);
    //         await connection.OpenAsync();

    //         var command = new SqlCommand("UpdateBookByAdmin", connection)
    //         {
    //             CommandType = CommandType.StoredProcedure
    //         };
    //         // Thêm các tham số cho thủ tục

    //         await command.ExecuteNonQueryAsync();
    //         return Ok(new { message = "Book updated successfully." });
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new { message = ex.Message });
    //     }
    // }

    [Authorize(Roles = "admin")]
    // DELETE: /books/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        try
        {
            // Lấy token từ header Authorization
            string authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header" });
            }

            string token = authHeader["Bearer ".Length..].Trim();
            if (!_jwt.ValidateToken(token, out ClaimsPrincipal? claims))
            {
                return Unauthorized(new { message = "Token is invalid" });
            }

            var userName = claims?.FindFirst(c => c.Type == "UserName")?.Value;

            // Lấy UserID từ tên người dùng
            int adminUserId;
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                using (var sqlCommand = new SqlCommand("SELECT UserID FROM Users WHERE UserName = @UserName", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@UserName", userName);
                    var result = await sqlCommand.ExecuteScalarAsync();
                    if (result == null)
                    {
                        return NotFound(new { message = "User not found." });
                    }
                    adminUserId = (int)result;
                }
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new()
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "DeleteBookByAdmin"
            };

            command.Parameters.AddWithValue("@AdminUserID", adminUserId);
            command.Parameters.AddWithValue("@BookID", id);

            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Book deleted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // [HttpDelete("delete/{id}")]
    // public async Task<ActionResult> DeleteBook(int id)
    // {
    //     try
    //     {
    //         using var connection = new SqlConnection(_connectionString);
    //         await connection.OpenAsync();

    //         var command = new SqlCommand("DeleteBookByAdmin", connection)
    //         {
    //             CommandType = CommandType.StoredProcedure
    //         };
    //         command.Parameters.AddWithValue("@BookID", id);

    //         await command.ExecuteNonQueryAsync();
    //         return Ok(new { message = "Book deleted successfully." });
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(new { message = ex.Message });
    //     }
    // }
}