using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Admin() { }
        public Admin(int AdminId, string Username, string Password)
        {
            this.AdminId = AdminId;
            this.Username = Username;
            this.Password = Password;
        }
    }
}
