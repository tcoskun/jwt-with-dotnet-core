using System;

namespace JwtApplication.Services
{
    public class SecurityService : ISecurityService
    {
        public Guid Login(string username, string password)
        {
            //authentication method
            var constUsername = "test";
            var constPassword = "123";
            var constUserId = Guid.Parse("5121a6bd77f44ebdbf5b4db2ae1f3242");

            return username == constUsername && password == constPassword ? constUserId : default;
        }
    }

    public interface ISecurityService
    {
        Guid Login(string username, string password);
    }
}
