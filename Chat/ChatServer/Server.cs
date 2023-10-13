using System.Net.Sockets;
using System.Net;
using ChatServer.Interfaces;

namespace ChatServer
{
    /// <summary>
    /// Represents the chat server
    /// </summary>
    public class Server : IServer
    {
        /// <summary>
        /// Listening server
        /// </summary>
        private TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
        /// <summary>
        /// All connections
        /// </summary>
        public List<ChatClient> clients = new List<ChatClient>();
        /// <summary>
        /// Contains the name of the chat, which consists of nicknames of users who take part in the conversation
        /// </summary>
        public string ChatName { get; set; }
        /// <summary>
        /// Contains a list of client IDs
        /// </summary>
        public static List<string> clientId = new List<string>();

        /// <summary>
        /// Removes a closed connection by id from the list of connections
        /// </summary>
        /// <param name="id">Contains the client ID</param>
        public void RemoveConnection(string id)
        {
            ChatClient? client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                client?.Close();
            }
        }
        /// <summary>
        /// Listens for incoming connections
        /// </summary>
        public async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Server is running");
                

                while (true)
                {
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    IServerDbContext context = new ServerDbContext();
                    ChatClient chatClient = new ChatClient(tcpClient, this, context);
                    clients.Add(chatClient);
                    
                    Task.Run(chatClient.ProcessAsync);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            finally
            {
                Disconnect();                
            }
        }
        /// <summary>
        /// Broadcasts messages to connected clients
        /// </summary>
        /// <param name="message">Contains the client's message</param>
        /// <param name="id">Contains the client ID</param>
        /// <returns></returns>
        public async Task BroadcastMessageAsync(string message, string id)
        {
            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    await client.Writer.WriteLineAsync(message);
                    await client.Writer.FlushAsync();
                }
            }
        }

        /// <summary>
        /// Disconnects all clients
        /// </summary>
        public void Disconnect()
        {
            foreach (var client in clients)
            {
                client.Close();
            }
            tcpListener.Stop();
        }

    }
}
