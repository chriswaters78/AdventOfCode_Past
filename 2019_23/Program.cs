using IntCode;
using System.Net;

const char NL = (char)10;

var input = File.ReadAllText("input.txt").Split(",").Select(long.Parse).ToArray();

var enumerators = Enumerable.Range(0, 50).Select(i => new QueueEnumerator(new long[] { i }, -1)).ToArray();

var computers = Enumerable.Range(0, 50)
    .Select(i => new Computer($"NIC{i}", input.ToArray(), enumerators[i], false));

object globalLock = new object();

ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = 51 };
(long x, long y) lastNATSent = (-1,-1);
(long x, long y) lastNAT = (-1, -1);

Parallel.ForEach(computers.Append(new Computer("NAT", new long[0], null, false)), computer =>
{
    if (computer.Name == "NAT")
    {
        while (true)
        {
            //check idle, if so, sent lastNAT packet
            if (enumerators.All(qe => qe.Queue.Count == 0))
            {
                if (lastNATSent.y == lastNAT.y)
                {
                    Console.WriteLine($"Sent {lastNAT.y} from NAT twice in a row");
                    throw new Exception("STOP");
                }
                lastNATSent = lastNAT;
                lastNAT = (-1, -1);
                Console.WriteLine($"Sending {lastNATSent.x},{lastNATSent.y} from NAT");
                lock (globalLock)
                {
                    enumerators[0].Queue.Enqueue(lastNATSent.x);
                    enumerators[0].Queue.Enqueue(lastNATSent.y);
                }
                //need a bit of a pause here to ensure other comps are truly idle
                //without this we could have to ensure they had also received a -1 for their last input
                Thread.Sleep(1000);
            }
        }
    }
    else
    {
        bool cont = true;
        while (cont)
        {
            computer.MoveNext();
            var address = computer.Current;
            computer.MoveNext();
            var x = computer.Current;
            cont = computer.MoveNext();
            var y = computer.Current;

            if (address == 255)
            {
                //Console.WriteLine($"ADDRESS 255: {y}");
                lastNAT = (x,y);
            }
            else
            {
                lock (globalLock)
                {
                    //Console.WriteLine($"PACKET SENT - A:{address} X:{x} Y:{y}");
                    enumerators[address].Queue.Enqueue(x);
                    enumerators[address].Queue.Enqueue(y);
                }
            }
        }
    }
});

