using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class WaitApproveController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public WaitApproveController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public class RejectionRequest
    {
        public string Reason { get; set; }
    }

    // Lấy danh sách rentals chờ duyệt
    [Authorize(Roles = "admin")]
    [HttpGet("pending")]
    public async Task<ActionResult> GetPendingRentals()
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            string query = @"
            SELECT r.*, b.Title, b.Author, b.Description, b.ISBN, b.CoverImage
            FROM Rentals r
            JOIN Books b ON r.BookID = b.BookID
            WHERE r.Status = 'pending'";

            using SqlCommand command = new(query, connection);
            SqlDataAdapter da = new(command);
            DataTable dt = new();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return NotFound(new { message = "No rentals found in pending status." });
            }

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(new { data = dt }),
                ContentType = "application/json",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    // Duyệt yêu cầu mượn sách
    [Authorize(Roles = "admin")]
    [HttpPost("approve/{rentalId}")]
    public async Task<ActionResult> ApproveRental(int rentalId)
    {
        Console.WriteLine($"Received rentalId: {rentalId}");
        try
        {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            // Truyền ID của admin vào stored procedure
            // string query = @"
            // EXEC ApproveRentalRequest @RentalID = @RentalID";


            using SqlCommand command = new("ApproveRentalRequest", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RentalID", rentalId);

            Console.WriteLine($"Rental id: {rentalId}");
            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Rental request approved successfully." });
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Từ chối yêu cầu mượn sách
    [Authorize(Roles = "admin")]
    [HttpPost("reject/{rentalId}")]
    public async Task<ActionResult> RejectRental(int rentalId, [FromBody] RejectionRequest rejectionRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID người dùng từ token

        if (string.IsNullOrEmpty(rejectionRequest?.Reason))
        {
            return BadRequest(new { message = "Rejection reason is required." });
        }

        Console.WriteLine($"Received rentalId for rejection: {rentalId}, reason: {rejectionRequest}, userID: {userId}");
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new("RejectRentalRequest", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@RentalID", rentalId);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@RejectionReason", rejectionRequest.Reason);


            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Rental request rejected successfully." });
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
