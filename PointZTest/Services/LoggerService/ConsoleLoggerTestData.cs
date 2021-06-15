using System.Collections;
using System.Collections.Generic;

namespace PointZTest.Services.LoggerService
{
    public class ConsoleLoggerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {"abc ABC 123 !\"# @£$"};
            yield return new object[] {"abc ABC 123 !\"# @£$\nTest"};
            yield return new object[] {"abc ABC 123 !\"# @£$\nTest\tTest"};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}