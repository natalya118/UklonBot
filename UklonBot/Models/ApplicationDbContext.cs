using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UklonBot.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("UklonBotConn")
        {
        }

        public DbSet<User> Users { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}