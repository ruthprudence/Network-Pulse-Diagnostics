using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class TcpClientProgram : IDisposable
{
    private TcpClient? _client;
    private bool _disposed = false;

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync("127.0.0.1", 13000, cancellationToken);

            using var stream = _client.GetStream();

            string diagnosticMessage = "Diagnostic data from client";
            byte[] data = Encoding.ASCII.GetBytes(diagnosticMessage);
            await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            Console.WriteLine("Sent: {0}", diagnosticMessage);

            byte[] response = new byte[256];
            int bytesRead = await stream.ReadAsync(response, 0, response.Length, cancellationToken);
            string responseData = Encoding.ASCII.GetString(response, 0, bytesRead);
            Console.WriteLine($"Received: {responseData}");
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
            throw;
        }
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await ConnectAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _client?.Dispose();
            }

            _disposed = true;
        }
    }
}