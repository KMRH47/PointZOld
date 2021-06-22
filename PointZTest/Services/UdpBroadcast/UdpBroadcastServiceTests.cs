using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PointZTest.Services.UdpBroadcast
{
    public class UdpBroadcastServiceTests
    {
        private readonly UdpClient udpClient;
        private readonly ITestOutputHelper testOutputHelper;

        public UdpBroadcastServiceTests(UdpClient udpClient, ITestOutputHelper testOutputHelper)

        {
            this.testOutputHelper = testOutputHelper;
            this.udpClient = udpClient;
            udpClient.Client.Bind(new IPEndPoint(0, 45454));
        }

        [Fact]
        public async Task SuccessfullySendsUdpBroadcasts()
        {
            // Arrange
            string hostName = Dns.GetHostName();
            await this.udpClient.Client.ConnectAsync(IPAddress.Broadcast, 0);

            if (this.udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new NullReferenceException("LocalEndPoint is null.");

            string localEthernetAddress = localEndPoint.Address.ToString();
            IPEndPoint broadcastAddress = new(IPAddress.Broadcast, 45454);
            string message = $"[{hostName}] {localEthernetAddress}";
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            // Act
            this.testOutputHelper.WriteLine("Sending...");
            Task<int> sendTask = this.udpClient.SendAsync(bytes, bytes.Length, broadcastAddress);
            await sendTask;
            this.testOutputHelper.WriteLine($"\"{message}\" sent!");

            // Assert
            Assert.True(sendTask.IsCompletedSuccessfully);
        }
    }
}