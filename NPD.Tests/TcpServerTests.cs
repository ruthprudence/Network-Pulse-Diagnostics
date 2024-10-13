using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NPD.Tests
{
    public partial class ProgramTests
    {
        [Fact]
        public async Task TestTcpServer_RunAsync_DoesNotThrow()
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, timeout.Token);

            var serverTask = _server.RunAsync(cts.Token);
            await Task.Delay(1000); // Give the server time to start

            cts.Cancel(); // Stop the server
            await Assert.ThrowsAsync<OperationCanceledException>(() => serverTask);
        }

        [Fact]
        public async Task TestTcpServer_RunAsync_ReceivesDataFromClient()
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, timeout.Token);

            var serverTask = _server.RunAsync(cts.Token);
            await Task.Delay(1000); // Give the server time to start

            await _client.RunAsync(cts.Token);

            await Task.Delay(1000); // Give the client time to send the data

            cts.Cancel(); // Stop the server
            await Assert.ThrowsAsync<OperationCanceledException>(() => serverTask);

            Assert.Equal("Diagnostic data from client", _server.ReceivedData);
        }
    }
}