using System.Web.Security;
using Pressford.Code.Interfaces;

namespace Pressford.Code
{
    public class AuthenticationService : IAuthenticationService
    {
        public void Login(string email)
        {
            FormsAuthentication.SetAuthCookie(email, false);
        }
    }
}