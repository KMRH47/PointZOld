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
        public async Task ReceivesExpectedDataAsync()
        {
            // Arrange
            Task<UdpReceiveResult> receiveResultTask = this.udpClient.ReceiveAsync();
            const string expected = "ReceivesExpectedDataAsync";
            byte[] bytes = Encoding.UTF8.GetBytes(expected);
            IPEndPoint endPoint = new(IPAddress.Loopback, 45454);
            
            // Act
            await this.udpClient.SendAsync(bytes, bytes.Length, endPoint);
            UdpReceiveResult result = await receiveResultTask;
            string actual = Encoding.UTF8.GetString(result.Buffer);

            // Assert
            Assert.Equal(expected, actual);

            // Output
            this.testOutputHelper.WriteLine($"Expected: {expected}\n" +
                                            $"Actual: {actual}");
        }
    }
}