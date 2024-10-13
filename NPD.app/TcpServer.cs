using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class TcpServer : IDisposable
{
    private TcpListener? _server;
    private bool _disposed = false;
    private CancellationTokenSource? _cts;
    public string? ReceivedData { get; private set; }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        try
        {
            _server = new TcpListener(IPAddress.Any, 13000);
            _server.Start();
            Console.WriteLine("Server started...");

            while (!_cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = await _server.AcceptTcpClientAsync(_cts.Token);
                Console.WriteLine("Connected!");

                _ = Task.Run(async () =>
                {
                    using var stream = client.GetStream();

                    byte[] buffer = new byte[256];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                    ReceivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {ReceivedData}");

                    byte[] response = Encoding.ASCII.GetBytes("Data received");
                    await stream.WriteAsync(response, 0, response.Length, _cts.Token);
                }, _cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Server operation cancelled.");
            throw;
        }
        finally
        {
            Stop();
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        _server?.Stop();
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await StartAsync(cancellationToken);
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
                Stop();
                _cts?.Dispose();
            }

            _disposed = true;
        }
    }
}