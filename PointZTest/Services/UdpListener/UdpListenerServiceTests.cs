using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PointZ.Services.UdpBroadcast;
using Xunit;
using Xunit.Abstractions;

namespace PointZTest.Services.UdpListener
{
    public class UdpListenerServiceTests
    {
        private readonly UdpClient udpClient;
        private readonly IUdpBroadcastService udpBroadcastService;
        private readonly ITestOutputHelper testOutputHelper;

        public UdpListenerServiceTests(UdpClient udpClient, IUdpBroadcastService udpBroadcastService,
            ITestOutputHelper testOutputHelper)
        {
            this.udpClient = udpClient;
            this.udpBroadcastService = udpBroadcastService;
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task CanReceiveUdpBroadcast()
        {
            // Arrange
            CancellationTokenSource cancellationTokenSource = new();
            this.udpClient.Client.Bind(new IPEndPoint(0, 45454));

            // Act
            Task<UdpReceiveResult> receiveTask = this.udpClient.ReceiveAsync();
            _ = this.udpBroadcastService.StartAsync(cancellationTokenSource.Token);
            await receiveTask;
            byte[] resultBuffer = receiveTask.Result.Buffer;
            string receivedMessage = Encoding.UTF8.GetString(resultBuffer);

            // Assert
            Assert.NotNull(receivedMessage);
            Assert.NotEqual("", receivedMessage);
            this.testOutputHelper.WriteLine($"Received message: {receivedMessage}");

            // Finally
            cancellationTokenSource.Cancel();
        }
    }
}