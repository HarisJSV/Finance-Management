using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace entity
{
    [Table("USERS")] // matches your SQL table
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("email")]
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

