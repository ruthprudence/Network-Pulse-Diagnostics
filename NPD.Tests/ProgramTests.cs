using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NPD.Tests
{
    public partial class ProgramTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly TcpServer _server;
        private readonly TcpClientProgram _client;
        private readonly CancellationTokenSource _cts;

        public ProgramTests(TestFixture fixture)
        {
            _server = fixture.Server;
            _client = fixture.Client;
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _server?.Dispose();
            _client?.Dispose();
        }
    }
}