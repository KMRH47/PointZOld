using System.Collections;
using System.Collections.Generic;

namespace PointZTest.Services.DataInterpreter
{
    public class DataInterpreterTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {2};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}