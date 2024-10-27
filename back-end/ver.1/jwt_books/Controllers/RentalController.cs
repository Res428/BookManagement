using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

[ApiController]
[Route("[controller]")]
public class RentalController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public RentalController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public class Rental
    {
        public int UserID { get; set; }
        public int BookID { get; set; }
        // Thêm các thuộc tính khác
    }

    [AllowAnonymous]
    // POST: /rental/create
    [HttpPost("create")]
    public async Task<ActionResult> CreateRental([FromBody] JObject rentalData)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new()
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "CreateRental"
            };

            command.Parameters.AddWithValue("@UserID", rentalData["UserID"]?.Value<int>());
            command.Parameters.AddWithValue("@BookID", rentalData["BookID"]?.Value<int>());
            command.Parameters.AddWithValue("@DueDate", rentalData["DueDate"]?.Value<DateTime>());
            command.Parameters.AddWithValue("@TotalRentalCost", rentalData["TotalRentalCost"]?.Value<decimal>());

            await command.ExecuteNonQueryAsync();

            return Ok(new { message = "Rental created successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    // GET: /rental/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult> GetUserRentals(int userId)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            using SqlCommand command = new()
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetUserRentals"
            };

            command.Parameters.AddWithValue("@UserID", userId);

            SqlDataAdapter da = new(command);
            DataTable dt = new();
            da.Fill(dt);

            return Ok(new { data = dt });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
