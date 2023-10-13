namespace ChatServer.Interfaces
{
    public interface IServer
    {
        /// <summary>
        /// Removes a closed connection by id from the list of connections
        /// </summary>
        /// <param name="id">Contains the client ID</param>
        public void RemoveConnection(string id);

        /// <summary>
        /// Listens for incoming connections
        /// </summary>
        public Task ListenAsync();

        /// <summary>
        /// Broadcasts messages to connected clients
        /// </summary>
        /// <param name="message">Contains the client's message</param>
        /// <param name="id">Contains the client ID</param>
        /// <returns></returns>
        public Task BroadcastMessageAsync(string message, string id);

        /// <summary>
        /// Disconnects all clients
        /// </summary>
        public void Disconnect();
    }
}
