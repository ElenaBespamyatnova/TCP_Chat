using System.Net.Sockets;
using ChatServer.Interfaces;
using ChatServer.Models;

namespace ChatServer
{
    /// <summary>
    /// Represents a connection to the server of an individual client
    /// </summary>
    public class ChatClient : IChatClient
    {
        /// <summary>
        /// Property that defines the connection ID
        /// </summary>
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        /// <summary>
        /// StreamWriter object for sending messages
        /// </summary>
        protected internal StreamWriter Writer { get; }
        /// <summary>
        /// StreamReader object for receiving messages
        /// </summary>
        protected internal StreamReader Reader { get; }
        /// <summary>
        /// IServerDbContext interface object for accessing the database context
        /// </summary>
        protected readonly IServerDbContext db;
        /// <summary>
        /// TcpClient object to interact with the connected client
        /// </summary>
        protected readonly TcpClient client;
        /// <summary>
        /// Server class instance
        /// </summary>
        protected readonly Server server;

        public ChatClient(TcpClient tcpClient, Server chatServer, IServerDbContext dbContext)
        {

            client = tcpClient;
            server = chatServer;

            var stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
            db = dbContext;
        }

        /// <summary>
        /// Implements messaging with the client
        /// </summary>
        public async Task ProcessAsync()
        {
            try
            {
                string? userName = await Reader.ReadLineAsync();
                string? message = $"{userName} entered the chat.";
                server.ChatName += userName + "+";
                Server.clientId.Add(Id + ":" + userName);
                
                await server.BroadcastMessageAsync(message, Id);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        UserMessage msg = new UserMessage();
                        msg.MessageBody = await Reader.ReadLineAsync();
                        
                        message = msg.MessageBody;
                        if (message == null)
                        {
                            continue;
                        }
                        
                        msg.UserName = userName;
                        message = $"{userName}: {message}";
                        Console.WriteLine(message);
                        await SaveHistory(msg);
                        await server.BroadcastMessageAsync(message, Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        message = $"{userName} left the chat.";
                        Console.WriteLine(message);
                        await server.BroadcastMessageAsync(message, Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                server.RemoveConnection(Id);
            }
        }

        /// <summary>
        /// Saves messages history to database
        /// </summary>
        /// <param name="message">Contains the text of the user's message</param>
        public async Task SaveHistory(UserMessage message)
        {
            await db.UserMessages.AddAsync(new UserMessage { DateTimeMessage = message.DateTimeMessage, MessageBody = message.MessageBody, UserName = message.UserName, ChatName = server.ChatName, ClientId = Id });
            db.Save((ServerDbContext)db);
        }

        /// <summary>
        /// Сloses connections
        /// </summary>
        public void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }

}
