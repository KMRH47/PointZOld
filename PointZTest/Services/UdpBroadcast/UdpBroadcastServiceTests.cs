using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PointZTest.Services.UdpBroadcastService
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
        public async Task ReceivesExpectedDataAsync()
        {
            // Arrange
            Task<UdpReceiveResult> receiveResultTask = this.udpClient.ReceiveAsync();
            const string expected = "ReceivesExpectedDataAsync";

            // Act
            await SendAsync(expected, Encoding.UTF8);
            UdpReceiveResult result = await receiveResultTask;
            string actual = Encoding.UTF8.GetString(result.Buffer);

            // Assert
            Assert.Equal(expected, actual);

            // Output
            this.testOutputHelper.WriteLine($"Expected: {expected}\n" +
                                            $"Actual: {actual}");
        }

        private async Task SendAsync(string message, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(message);
            IPEndPoint endPoint = new(IPAddress.Loopback, 45454);
            await this.udpClient.SendAsync(bytes, bytes.Length, endPoint);
        }
    }
}