using Dapper;
using MySql.Data.MySqlClient;
using vocabQuizAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace vocabQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ContactController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] ContactRequest request)
        {
            using (var db = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                // 1nf ihmal eden datamiz, birden cok info tek alanda
                string query = @"
                    INSERT INTO contact_requests (user_name, message, contact_info_combined)
                    VALUES (@UserName, @Message, @ContactInfoCombined)";

                await db.ExecuteAsync(query, request);
                return Ok(new { message = "Request sent (Unnormalized)" });
            }
        }
    }
}
