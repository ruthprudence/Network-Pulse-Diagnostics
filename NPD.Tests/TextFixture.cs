using System;

namespace NPD.Tests
{
    public class TestFixture : IDisposable
    {
        public TcpServer Server { get; private set; }
        public TcpClientProgram Client { get; private set; }

        public TestFixture()
        {
            Server = new TcpServer();
            Client = new TcpClientProgram();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}