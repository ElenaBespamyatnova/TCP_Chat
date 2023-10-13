using ChatServer.Models;

namespace ChatServer.Interfaces
{
    public interface IChatClient
    {
        /// <summary>
        /// Implements messaging with the client
        /// </summary>
        public Task ProcessAsync();

        /// <summary>
        /// Saves messages history to database
        /// </summary>
        /// <param name="message">Contains the text of the user's message</param>
        public Task SaveHistory(UserMessage message);

        /// <summary>
        /// Сloses connections
        /// </summary>
        public void Close();
    }
}
