using Client.Models;
using Microsoft.EntityFrameworkCore;

namespace Client.Interfaces
{
    public interface IChatDbContext : IDisposable
    {
        /// <summary>
        /// Set of User class objects that are stored in the database
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Saves data to a database
        /// </summary>
        /// <param name="context">Data context used to interact with the database</param>
        public void Save(ChatDbContext context);
    }
}
