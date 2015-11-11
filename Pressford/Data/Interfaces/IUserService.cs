using Pressford.Data.Dtos;

namespace Pressford.Data.Interfaces
{
    public interface IUserService
    {
        User LoginUser(string email, string password);
        bool IsUserAdmin(string email);
    }
}