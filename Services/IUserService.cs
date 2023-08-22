using BlazorAppLogin.Dtos;
using BlazorAppLogin.Entities;

namespace BlazorAppLogin.Services
{
    public interface IUserService
	{
		List<UserDto> Users { get; set; }
		LoginDto Login { get; set; }
		Task GetUsers();
		Task<bool> GetSingleUser(LoginDto logn);
		Task CreateUser(UserDto user);
		Task DeleteUser(int Id);
	}
}