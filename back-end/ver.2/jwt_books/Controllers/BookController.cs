using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace jwt_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string ISBN { get; set; }
            public DateTime PublishedDate { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public bool IsAvailableForRent { get; set; }
            public decimal RentPrice { get; set; }
            public int CategoryID { get; set; }
            public string CoverImage { get; set; }
        }

        public class BookUpdateModel : Book
        {
            public int BookID { get; set; }
        }

        // Lấy danh sách sách
        [HttpGet("books")]
        public async Task<ActionResult> GetBooks()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                using SqlCommand command = new();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Books";
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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Thêm sách
        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<ActionResult> AddBook([FromBody] Book bookData)
        {
            if (bookData == null)
            {
                return BadRequest(new { message = "Book data is required." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                using SqlCommand command = new("AddBookByAdmin", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Title", bookData.Title);
                command.Parameters.AddWithValue("@Author", bookData.Author);
                command.Parameters.AddWithValue("@Description", bookData.Description);
                command.Parameters.AddWithValue("@ISBN", bookData.ISBN);
                command.Parameters.AddWithValue("@PublishedDate", bookData.PublishedDate);
                command.Parameters.AddWithValue("@Price", bookData.Price);
                command.Parameters.AddWithValue("@StockQuantity", bookData.StockQuantity);
                command.Parameters.AddWithValue("@IsAvailableForRent", bookData.IsAvailableForRent);
                command.Parameters.AddWithValue("@RentPrice", bookData.RentPrice);
                command.Parameters.AddWithValue("@CategoryID", bookData.CategoryID);
                command.Parameters.AddWithValue("@CoverImage", bookData.CoverImage);

                var newBookId = await command.ExecuteScalarAsync();

                return CreatedAtAction(nameof(GetBooks), new { id = newBookId }, new { message = "Book added successfully.", BookID = newBookId });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new { message = "Database error: " + sqlEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Sửa sách
        [Authorize(Roles = "admin")]
        [HttpPost("update/{bookId}")]
        public async Task<ActionResult> UpdateBook(int bookId, [FromBody] BookUpdateModel bookData)
        {
            if (bookData == null || bookId <= 0)
            {
                return BadRequest(new { message = "Valid BookID is required." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                using SqlCommand command = new("UpdateBookByAdmin", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@BookID", bookId);
                command.Parameters.AddWithValue("@Title", bookData.Title);
                command.Parameters.AddWithValue("@Author", bookData.Author);
                command.Parameters.AddWithValue("@Description", bookData.Description);
                command.Parameters.AddWithValue("@Price", bookData.Price);
                command.Parameters.AddWithValue("@StockQuantity", bookData.StockQuantity);
                command.Parameters.AddWithValue("@IsAvailableForRent", bookData.IsAvailableForRent);
                command.Parameters.AddWithValue("@RentPrice", bookData.RentPrice);
                command.Parameters.AddWithValue("@CoverImage", bookData.CoverImage);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound(new { message = "No book found with the given BookID." });
                }

                return Ok(new { message = "Book updated successfully." });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new { message = "Database error: " + sqlEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Xóa sách
        [Authorize(Roles = "admin")]
        [HttpPost("delete/{bookId}")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            if (bookId <= 0)
            {
                return BadRequest(new { message = "Valid BookID is required." });
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                using SqlCommand command = new("DeleteBookByAdmin", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@BookID", bookId);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound(new { message = "No book found with the given BookID." });
                }

                return Ok(new { message = "Book deleted successfully." });
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new { message = "Database error: " + sqlEx.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
