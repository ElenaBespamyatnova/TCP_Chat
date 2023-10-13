using Client.Interfaces;
using Client.Models;
using Microsoft.EntityFrameworkCore;


namespace Client
{
    public class ChatDbContext : DbContext, IChatDbContext
    {
        /// <summary>
        /// Set of User class objects that are stored in the database
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;
        /// <summary>
        /// Overriding the OnConfiguring method to configure a connection to the database
        /// </summary>
        /// <param name="optionsBuilder">Allows to configure the connection string for connecting to the database</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=ChatUsers;Trusted_Connection=True;");
        }
        /// <summary>
        /// Saves data to a database
        /// </summary>
        /// <param name="context">Data context used to interact with the database</param>
        public void Save(ChatDbContext context)
        {
            context.SaveChanges();
        }
    }
}
