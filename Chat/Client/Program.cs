using ChatServer;
using ChatServer.Interfaces;
using Client.Interfaces;

namespace Client
{
    internal partial class Program
    {
        public static async Task Main(string[] args)
        {
            IChatDbContext db = new ChatDbContext();
            IServerDbContext serverDb = new ServerDbContext();
            Client client = new Client(db, serverDb);
            await client.StartMenu();            
        }
    }
}

        
