using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCode
{
    public class ValueEnumerator : IEnumerator<long>
    {
        public Func<long,long> ValueProducer = value => value;
        public long Value = 0;
        public long Current => ValueProducer(Value);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            Console.WriteLine($"ValueEnumerator returned {Value}");
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
