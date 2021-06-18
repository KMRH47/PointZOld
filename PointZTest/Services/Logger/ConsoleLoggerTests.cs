using System;
using System.IO;
using Xunit;

namespace PointZTest.Services.Logger
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
        [ClassData(typeof(ConsoleLoggerTestData))]
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