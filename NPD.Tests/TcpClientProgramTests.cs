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
        public async Task TestTcpClientProgram_RunAsync_ThrowsSocketException_WhenServerIsNotRunning()
        {
            await Assert.ThrowsAsync<SocketException>(() => _client.RunAsync());
        }
    }
}