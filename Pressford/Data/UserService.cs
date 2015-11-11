using System.Linq;
using Pressford.Data.Dtos;
using Pressford.Data.Interfaces;

namespace Pressford.Data
{
    public class UserService : IUserService
    {
        private readonly PressfordModel _db;

        public UserService(PressfordModel db)
        {
            _db = db;
        }

        public User LoginUser(string email, string password)
        {
            return _db.Users.SingleOrDefault(x => x.Email == email && x.Password == password);
        }

        public bool IsUserAdmin(string email)
        {
            return _db.Users.Single(x => x.Email == email).IsAdmin;
        }
    }
}