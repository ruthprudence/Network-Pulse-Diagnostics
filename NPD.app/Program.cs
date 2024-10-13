using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var client = new TcpClientProgram();
        var server = new TcpServer();

        _ = Task.Run(async () => await server.RunAsync());
        await Task.Delay(1000);
        await client.RunAsync();
    }
}