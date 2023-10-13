using ChatServer;


public class Program
{
    public static async Task Main(string[] args)
    {
        Server server = new Server();
        await server.ListenAsync();
    }
}




