using System;
using System.IO;
using Xunit;

namespace PointZTest.Services.LoggerService
{
    public class ConsoleLoggerTests
    {
        private readonly TextWriter defaultWriter;
        private readonly StringWriter testWriter;

        public ConsoleLoggerTests(StringWriter testWriter)
        {
            this.defaultWriter = Console.Out;
            this.testWriter = testWriter;
        }

        [Theory]
        [InlineData("abc ABC 123 !\"# @£$")]
        [InlineData("abc ABC 123 !\"# @£$\nTest")]
        [InlineData("abc ABC 123 !\"# @£$\nTest\tTest")]
        public void OutputsCorrectlyToConsole(string expected)
        {
            // arrange
            Console.SetOut(this.testWriter);
            
            // act
            this.testWriter.WriteLine(expected);
            var actual = this.testWriter.ToString();
            
            // assert
            Assert.Contains(expected, actual);
            
            // finalize
            Console.SetOut(this.defaultWriter);
        }
    }
}