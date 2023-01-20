class Program
{
    public static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt").Select(ch => int.Parse(ch.ToString())).ToArray();

        Node[] cups = Enumerable.Repeat(0,input.Length).Select( _ => new Node()).ToArray();
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].Value = input[i];
        }

        var minValue = cups.Min(n => n.Value);
        var maxValue = cups.Max(n => n.Value);

        cups = cups.Concat(Enumerable.Range(maxValue + 1, 1000000 - cups.Length).Select(i => new Node() { Value = i })).ToArray();
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].Next = cups[(i + 1) % cups.Length];
        }
        maxValue = cups.Max(n => n.Value);

        var valToNodeMap = cups.ToDictionary(node => node.Value, node => node);

        Node current = cups[0];
        for (int m = 0; m < 10000000; m++)
        {
            //pick up three cups (index + 1, index + 2, index + 3
            var cup1 = current.Next;
            var cup2 = cup1.Next;
            var cup3 = cup2.Next;
            current.Next = cup3.Next;


            var destinationLabel = current.Value;
            do
            {
                destinationLabel = destinationLabel - 1;
                if (destinationLabel < minValue)
                {
                    destinationLabel = maxValue;
                }
            }
            while (cup1.Value == destinationLabel || cup2.Value == destinationLabel || cup3.Value == destinationLabel);

            var before = valToNodeMap[destinationLabel];
            var after = before.Next;
            before.Next = cup1;
            cup3.Next = after;

            current = current.Next;
        }


        var part1 = new List<string>();
        var cur = valToNodeMap[1].Next; 
        for (int i = 0; i < cups.Length - 1; i++)
        {
            part1.Add(cur.Value.ToString());
            cur = cur.Next;
        }

        var p1Str = String.Join("", part1);
        
        //712484270203?
        var part2 = $"{(long) valToNodeMap[1].Next.Value * (long) valToNodeMap[1].Next.Next.Value}";
    }
}

public class Node
{
    public Node Next;
    public int Value;
}