using db;
using entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace efrepo
{
    public class EfRepo
    {
        public List<User> GetAllUsers()
        {
            using (var context = new AppDbContext())
            {
                return context.Users.ToList();
            }
        }
    }
}
