using entity;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;


namespace db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext") { }

        public DbSet<User> Users { get; set; }
    }
}
