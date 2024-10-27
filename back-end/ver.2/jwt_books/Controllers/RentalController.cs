using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class RentalController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public RentalController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }


    // DTO cho thông tin mượn sách
    public class RentalDto
    {
        public int RentalID { get; set; }
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }

    // Define the CreateRentalRequest class
    public class CreateRentalRequest
    {
        // Properties for the rental request
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
    }
    
    // API yêu cầu mượn sách
    [Authorize]
    [HttpPost("request")]
    public async Task<ActionResult> RequestBook([FromBody] CreateRentalRequest rentalRequest)
    {
        if (rentalRequest == null)
        {
            return BadRequest(new { message = "Rental data is required." });
        }
        try
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new("RequestBookRental", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Thêm các tham số cho stored procedure
            command.Parameters.AddWithValue("@UserID", rentalRequest.UserID);
            command.Parameters.AddWithValue("@BookID", rentalRequest.BookID);
            command.Parameters.AddWithValue("@RentalDate", rentalRequest.RentalDate); // Ngày mượn sách
            command.Parameters.AddWithValue("@DueDate", rentalRequest.DueDate); // Ngày trả sách

            // Thực thi stored procedure
            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Book request submitted successfully." });
        }
        catch (SqlException sqlEx)
        {
            // Xử lý lỗi từ SQL
            return BadRequest(new { message = "SQL Error: " + sqlEx.Message });
        }
        catch (Exception ex)
        {
            // Xử lý lỗi chung
            return BadRequest(new { message = "Error requesting book rental: " + ex.Message });
        }
    }


    // API lấy danh sách sách mà người dùng đang mượn
    [Authorize]
    [HttpGet("borrowed-books/{userId}")]
    public async Task<ActionResult> GetUserBorrowedBooks(int userId)
    {
        // Kiểm tra xem userId có khớp với người dùng hiện tại không
        var currentUserId = GetCurrentUserId(); // Lấy UserID từ claims
        Console.WriteLine($"User ID from URL: {userId}, Current User ID: {currentUserId}");
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(new { message = "User not authenticated." });
        }


        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            string query = @"SELECT r.RentalID, b.BookID, b.Title, b.Author, b.CoverImage, r.RentalDate, r.DueDate, r.Status FROM Rentals r JOIN Books b ON r.BookID = b.BookID WHERE r.UserID = @UserID AND r.Status = 'rented'";


            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserID", currentUserId);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            var borrowedBooks = new List<RentalDto>();

            while (await reader.ReadAsync())
            {
                borrowedBooks.Add(new RentalDto
                {
                    RentalID = reader.GetInt32(0),
                    BookID = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Author = reader.GetString(3),
                    CoverImage = reader.IsDBNull(4) ? null : reader.GetString(4),
                    RentalDate = reader.GetDateTime(5),
                    DueDate = reader.GetDateTime(6),
                    Status = reader.GetString(7)
                });
            }

            return Ok(borrowedBooks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error retrieving borrowed books: " + ex.Message });
        }
    }

    // API trả sách
    [Authorize]
    [HttpPut("return/{rentalId}")]
    public async Task<ActionResult> ReturnBook(int rentalId)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            // Kiểm tra xem RentalID có tồn tại và thuộc về UserID hiện tại không
            using (SqlCommand cmd = new("SELECT COUNT(*) FROM Rentals WHERE RentalID = @RentalID AND UserID = @UserID AND Status = 'rented'", connection))
            {
                cmd.Parameters.AddWithValue("@RentalID", rentalId);
                cmd.Parameters.AddWithValue("@UserID", GetCurrentUserId());

                var result = await cmd.ExecuteScalarAsync();
                if (Convert.ToInt32(result) == 0)
                {
                    return NotFound(new { message = "No rental found for this RentalID." });
                }
            }

            // Gọi thủ tục lưu trữ để cập nhật trạng thái
            using SqlCommand command = new("ReturnRental", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RentalID", rentalId);

            await command.ExecuteNonQueryAsync();
            return Ok(new { message = "Book returned successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error returning book: " + ex.Message });
        }
    }

}
