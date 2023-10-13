using ChatServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Interfaces
{
    public interface IServerDbContext : IDisposable
    {
        /// <summary>
        /// Set of UserMessage class objects that are stored in the database
        /// </summary>
        public DbSet<UserMessage> UserMessages { get; set; }
        /// <summary>
        /// Saves data to a database
        /// </summary>
        /// <param name="context">Data context used to interact with the database</param>
        public void Save(ServerDbContext context);
    }
}
