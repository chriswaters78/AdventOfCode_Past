using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntCode
{
    public class CommandEnumerable : IEnumerable<long>
    {

        private IEnumerable<string> commands;
        public CommandEnumerable(IEnumerable<string> commands)
        {
            this.commands = commands;
        }

        private IEnumerable<long> GetEnumerable()
        {
            foreach (var command in commands)
            {
                foreach (var ch in command)
                {
                    yield return ch;
                }
                yield return 10;
            }
            yield break;
        }

        public IEnumerator<long> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
