using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity
{
    public class User
    {
        public int UserId { get; set; }              
        public string Username { get; set; }         
        public string Password { get; set; }         
        public string Email { get; set; }


        public User() { }


        public User(int userId, string username, string password, string email)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
