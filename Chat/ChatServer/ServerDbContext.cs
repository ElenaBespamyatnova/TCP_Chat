using ChatServer.Interfaces;
using ChatServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatServer
{
    public class ServerDbContext : DbContext, IServerDbContext
    {
        /// <summary>
        /// Set of UserMessage class objects that are stored in the database
        /// </summary>
        public DbSet<UserMessage> UserMessages { get; set; } = null!;

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
        public void Save(ServerDbContext context)
        {            
           context.SaveChanges();            
        }
    }
}

