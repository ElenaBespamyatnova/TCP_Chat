using System.Collections;

namespace Client.Interfaces
{
    public interface IClient
    {
        /// <summary>
        /// Implements the application's initial menu
        /// </summary>
        public Task StartMenu();
        /// <summary>
        /// Implements a menu with accessible functionality for the user
        /// </summary>
        public Task UserMenu();
        /// <summary>
        /// Prints the initial menu
        /// </summary>
        public void PrintInitialMenu();
        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="username">Contains username</param>
        /// <param name="password">Contains user's password</param>
        /// <param name="phone">Contains user's phone</param>
        public void UserRegistration(string username, string password, string phone);
        /// <summary>
        /// Authorizes the user
        /// </summary>
        /// <param name="username">Contains username</param>
        /// <param name="password">Contains user's password</param>
        /// <returns></returns>
        public Task UserAuthorization(string username, string password);
        /// <summary>
        /// Connects to the server
        /// </summary>
        public Task ConnectingToServerAsync();
        /// <summary>
        /// Sends messages
        /// </summary>
        /// <param name="writer">StreamWriter object for sending data</param>
        /// <returns></returns>
        public Task SendMessageAsync(StreamWriter writer);
        /// <summary>
        /// Receives messages
        /// </summary>
        /// <param name="reader">StreamReader object for reading data</param>
        /// <returns></returns>
        public Task ReceiveMessageAsync(StreamReader reader);
        /// <summary>
        /// Retrieves message history from the database
        /// </summary>
        /// <param name="interlocutorName">Contains the name of the interlocutor</param>
        /// <param name="userDate">Contains the date when the message was sent</param>
        /// <returns>Returns a list of messages with a specific user for a specific date</returns>
        public IEnumerable GetChatHistory(string interlocutorName, string userDate);
        /// <summary>
        /// Logs out the user
        /// </summary>
        public void LogOut();
        /// <summary>
        /// Prints messages to the console
        /// </summary>
        /// <param name="message">Contains the user's message</param>
        public void Print(string message);
    }
}
