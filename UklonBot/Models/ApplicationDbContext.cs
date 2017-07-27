using System.Data.Entity;

namespace UklonBot.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("UklonBotConn")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChannelUser> ChannelUsers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}