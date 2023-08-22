using BlazorAppLogin.Data;
using BlazorAppLogin.Dtos;
using BlazorAppLogin.Entities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppLogin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
        private readonly IConfiguration _config;
		public UserController(IConfiguration config)
		{
			_config = config;
		}
		public static User user = new User();
		[HttpGet("getusers")]
		public async Task<ActionResult<List<User>>> GetAllUsers()
		{
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			IEnumerable<User> users = await SellectAllUsers(connection);
			return Ok(users);
		}
        [HttpPost("login")]
        public async Task<ActionResult> GetUser([FromBody]LoginDto logn)
        {
			using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
			var user = await connection.QueryAsync<Login?>("select * from Users where email = @Email and password = @Password", new { Email = logn.Email, Password = logn.Password});
			if ( user.Count() > 0)
			{
				return Ok(true);
			}
			else
			{
				return Ok(false);
			}			
		}
        [HttpPost("createuser")]
		public async Task<ActionResult<List<User>>> CreateUser(User user)
		{
			try
			{
				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				await connection.ExecuteAsync("insert into Users (fullname, email, password, phonenumber, address) values (@FullName, @Email, @Password, @PhoneNumber, @Address)", user);
				return Ok(await SellectAllUsers(connection));
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpPut]
		public async Task<ActionResult<List<User>>> UpdateUser(User user)
		{
			try
			{
				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				await connection.ExecuteAsync("update users set fullname = @FullName, email = @Email, password = @Password, phonenumber = @PhoneNumber, address = @Address  where id = @Id", user);
				return Ok(await SellectAllUsers(connection));
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpDelete("{Id}")]
		public async Task<ActionResult<List<User>>> DeleteUser(int Id)
		{
			try
			{
				using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
				await connection.ExecuteAsync("delete from users where id = @Idd", new { Idd = Id });
				return Ok(await SellectAllUsers(connection));
			}
			catch (Exception)
			{
				throw;
			}
		}
		private static async Task<IEnumerable<User>> SellectAllUsers(SqlConnection connection)
		{
			return await connection.QueryAsync<User>("select * from Users");
		}
	}
}