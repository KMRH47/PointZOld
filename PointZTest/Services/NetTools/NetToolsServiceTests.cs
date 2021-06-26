using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PointZTest.Services.NetTools
{
    public class NetToolsServiceTests
    {
        private readonly UdpClient udpClient;
        private readonly ITestOutputHelper testOutputHelper;

        public NetToolsServiceTests(UdpClient udpClient, ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.udpClient = udpClient;
        }

        [Fact]
        public async Task GetsPrimaryNetworkInterfaceControllerIpv4Address()
        {
            // Arrange
            await this.udpClient.Client.ConnectAsync(IPAddress.Broadcast, 0);

            // Act
            if (this.udpClient.Client.LocalEndPoint is not IPEndPoint localEndPoint)
                throw new NullReferenceException("LocalEndPoint is null.");
            
            string localEndpointAddress = localEndPoint.Address.ToString();
            bool startsWith10 = localEndpointAddress.StartsWith("10");
            bool startsWith172 = localEndpointAddress.StartsWith("172");
            bool startsWith192 = localEndpointAddress.StartsWith("192");
            bool isPrivateAddress = startsWith10 || startsWith172 || startsWith192;

            // Assert
            Assert.Equal(AddressFamily.InterNetwork, localEndPoint.AddressFamily);
            this.testOutputHelper.WriteLine($"Expected: {AddressFamily.InterNetwork}\n" +
                                            $"Actual: {localEndPoint.AddressFamily}");
            Assert.True(isPrivateAddress);
            this.testOutputHelper.WriteLine($"Starts with 10: {startsWith10}\n" +
                                            $"Starts with 172: {startsWith172}\n" +
                                            $"Starts with 192: {startsWith192}");
        }
    }
}