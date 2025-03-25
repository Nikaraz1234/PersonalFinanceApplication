using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace PersonalFinanceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupaBaseController : ControllerBase
    {
        private readonly NpgsqlConnection _connection;
        public SupaBaseController(NpgsqlConnection connection)
        {
            _connection = connection;
        }


        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = new List<string>();

            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand("SELECT username FROM Users", _connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    users.Add(reader.GetString(0)); 
                }
            }

            return Ok(users);
        }
    }
}
