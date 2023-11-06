using System.Security;

namespace TestWebSocketApplication2.Models
{
    public class User {
        public string Email { get; set; }
        public string Password { get; set; }

        public User() { }

        public User SetEmail(string email) {
            Email = email;
            return this;
        }

        public User SetPassword(string password) {
            Password = password;
            return this;
        }
    }
}
