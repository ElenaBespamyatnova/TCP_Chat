using ChatServer.Interfaces;
using ChatServer.Models;
using Client.Interfaces;
using Client.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Globalization;
using System.Net.Sockets;

namespace Client
{
    /// <summary>
    /// Represents the client that will connect to the server
    /// </summary>
    public class Client : IClient
    {
        private readonly string host;
        private readonly int port;
        /// <summary>
        /// TcpClient object to interact with the connected client
        /// </summary>
        private static TcpClient? client;
        /// <summary>
        /// Chat username
        /// </summary>
        private static string? userName;
        /// <summary>
        /// StreamReader object for receiving messages
        /// </summary>
        private StreamReader? Reader;
        /// <summary>
        /// StreamWriter object for sending messages
        /// </summary>
        private StreamWriter? Writer;
        /// <summary>
        /// An instance of the user class representing the current user
        /// </summary>
        private User currentUser;
        /// <summary>
        /// IChatDbContext interface object for accessing the database context
        /// </summary>
        public readonly IChatDbContext context;
        /// <summary>
        /// IServerDbContext interface object for accessing the database context
        /// </summary>
        public readonly IServerDbContext serverDb;
       

        public Client(IChatDbContext dataContext, IServerDbContext serverContext)
        {

            context = dataContext;
            serverDb = serverContext;
            host = "127.0.0.1";
            port = 8888;
            client = new TcpClient();
        }

        /// <summary>
        /// Prints the initial menu
        /// </summary>
        public void PrintInitialMenu()
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Sign up");
            Console.WriteLine("3 - Stay as a guest");
        }

        /// <summary>
        /// Prints the user menu
        /// </summary>
        public void PrintUserMenu()
        {
            Console.WriteLine("1 - Get chat history");
            Console.WriteLine("2 - Log out");
        }
        /// <summary>
        /// Implements a menu with accessible functionality for the user
        /// </summary>
        public async Task UserMenu()
        {
            PrintUserMenu();
            int userChoice = 0;
            Console.Write("Choose an action: ");
            userChoice = int.Parse(Console.ReadLine());
            switch (userChoice)
            {
                case 1:
                    if (userName == "Guest")
                    {
                        Console.WriteLine("Sign in to receive the conversation history.");
                        LogOut();
                        client = new TcpClient();
                        await StartMenu();
                    }
                    Console.Write("Enter your contact's name: ");
                    string contactName = Console.ReadLine();
                    Console.Write("Enter the date of the conversation: ");
                    string dateConversation = Console.ReadLine();
                    GetChatHistory(contactName, dateConversation);
                    break;
                case 2:
                    Console.WriteLine("Log out");
                    LogOut();
                    client = new TcpClient();
                    await StartMenu();
                    await ConnectingToServerAsync();
                    break;
                default:
                    Console.WriteLine("Incorrect value. Select an existing action.");
                    await UserMenu();
                    break;
            }
        }
        /// <summary>
        /// Implements the application's initial menu
        /// </summary>
        public async Task StartMenu()
        {
            PrintInitialMenu();
            int userChoice = 0;
            string password = string.Empty;
            string phone = string.Empty;
            try
            {
                userChoice = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine($"Error: Incorrect input string");
                await StartMenu();
            }            

            switch (userChoice)
            {
                case 1:
                    Console.Write("Enter your name: ");
                    userName = Console.ReadLine();
                    Console.Write("Enter your password: ");
                    password = Console.ReadLine();

                    await UserAuthorization(userName, password);
                    await ConnectingToServerAsync();


                    break;
                case 2:
                    Console.Write("Enter your name: ");
                    userName = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();
                    Console.Write("Enter your phone number: ");
                    phone = Console.ReadLine();
                    UserRegistration(userName, password, phone);
                    Console.WriteLine($"Welcome, {userName}!");
                    await ConnectingToServerAsync();
                    break;
                case 3:
                    userName = "Guest";
                    Console.WriteLine($"Welcome!");
                    await ConnectingToServerAsync();
                    break;
                default: 
                    Console.WriteLine("Incorrect value. Select an existing action.");
                    await StartMenu();
                    break;
                    
            }
        }
        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="username">Contains username</param>
        /// <param name="password">Contains user's password</param>
        /// <param name="phone">Contains user's phone</param>
        public void UserRegistration(string username, string password, string phone)
        {
            try
            {
                User user;

                context.Users.AddAsync(user = new User { Name = username, Password = password, Phone = phone });
                context.Save((ChatDbContext)context);

                currentUser = user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                context.Dispose();
            }            
        }
        /// <summary>
        /// Authorizes the user
        /// </summary>
        /// <param name="username">Contains username</param>
        /// <param name="password">Contains user's password</param>
        /// <returns></returns>
        public async Task UserAuthorization(string username, string password)
        {
            try
            {
                User user = await context.Users.FirstOrDefaultAsync(u => u.Name == username && u.Password == password);

                if (user != null)
                {
                    user.IsAuthorized = true;
                    currentUser = user;
                    Console.WriteLine($"Welcome, {userName}!");
                }
                else
                {
                    Console.WriteLine("Invalid login or password :(");
                    await StartMenu();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                context.Dispose();
            }
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        public async Task ConnectingToServerAsync()
        {
            try
            {
                client.Connect(host, port);
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());

                if (Writer is null || Reader is null)
                    return;

                Task.Run(() => ReceiveMessageAsync(Reader));
                await SendMessageAsync(Writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Writer?.Close();
            Reader?.Close();
        }

        /// <summary>
        /// Sends messages
        /// </summary>
        /// <param name="writer">StreamWriter object for sending data</param>
        public async Task SendMessageAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            Console.WriteLine("To send a message, type your message and press Enter.");

            while (true)
            {
                Message message = new Message();

                message.MessageBody = Console.ReadLine();
                if (message.MessageBody == "menu")
                {
                    await UserMenu();
                    continue;
                }
                await writer.WriteLineAsync(message.MessageBody);                             
                await writer.FlushAsync();
            }
        }
        /// <summary>
        /// Receives messages
        /// </summary>
        /// <param name="reader">StreamReader object for reading data</param>
        public async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    Message message = new Message();

                    string answer = $"{message.DateTimeMessage}\n";

                    message.MessageBody = await reader.ReadLineAsync();
                    if (message.MessageBody == "menu".ToLower())
                    {
                       await UserMenu();
                    }
                    answer += message.MessageBody;
                    if (string.IsNullOrEmpty(message.MessageBody))
                        continue;
                    Print(answer);
                }
                catch
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Prints messages to the console
        /// </summary>
        /// <param name="message">Contains the user's message</param>
        public void Print(string message)
        {

            var position = Console.GetCursorPosition();
            int left = position.Left;
            int top = position.Top;

            Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
            Console.SetCursorPosition(0, top);
            Console.WriteLine(message);
            Console.SetCursorPosition(left, top + 2);
        }
        /// <summary>
        /// Retrieves message history from the database
        /// </summary>
        /// <param name="interlocutorName">Contains the name of the interlocutor</param>
        /// <param name="userDate">Contains the date when the message was sent</param>
        /// <returns>Returns a list of messages with a specific user for a specific date</returns>
        public IEnumerable GetChatHistory(string interlocutorName, string userDate)
        {

            var date = DateOnly.Parse(userDate).ToString("o", CultureInfo.InvariantCulture);
            var messages = new List<UserMessage>();
            using (serverDb)
            {
                messages = serverDb.UserMessages.Where(n => n.ChatName.Contains(userName)).Where(c => c.ChatName.Contains(interlocutorName)).Where(d => d.DateTimeMessage.Date.ToString().Contains(date)).ToList();
            }
            string? historyResult = null;
            foreach (var message in messages)
            {
                historyResult += $"{new String('-', 50)}\n{message.DateTimeMessage}\n{message.UserName}: {message.MessageBody}\n{new String('-', 50)}\n";
            }
            if (historyResult != null)
            {
                Console.WriteLine(historyResult);
            }else 
            {
                Console.WriteLine("Conversation not found!");
            }
            return messages;
        }
        /// <summary>
        /// Logs out the user
        /// </summary>
        public void LogOut()
        {
            client.Close();
            userName = null;
            if (currentUser != null)
            {
                currentUser.IsAuthorized = false;
            }
        }
    }
}
