using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCode
{
    public class QueueEnumerator : IEnumerator<long>
    {

        public ConcurrentQueue<long> Queue;
        private long defaultValue;
        private long current;
        public long Current => current;

        public QueueEnumerator(IEnumerable<long> initialQueue, long defaultValue)
        {
            this.Queue = new ConcurrentQueue<long>(initialQueue);
            this.defaultValue = defaultValue;
        }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (Queue.TryDequeue(out long result))
            {
                //Console.WriteLine($"Queue item returned: {result}");
                current = result;
            }
            else
            { 
                current = defaultValue;
            }
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
