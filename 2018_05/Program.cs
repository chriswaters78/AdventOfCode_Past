var input = File.ReadAllText("input.txt");

Console.WriteLine($"Part 1: {reduce(new LinkedList<char>(input)).Count}");
Console.WriteLine($"Part 2: {input.Select(ch => Char.ToLower(ch)).Distinct().Min(without => reduce(new LinkedList<char>(input.Where(ch => Char.ToLower(ch) != without))).Count)}");

LinkedList<char> reduce(LinkedList<char> list)
{
    var current = list.First;
    while (current.Next != null)
    {
        if ((char.ToLower(current.Value) == char.ToLower(current.Next.Value))
            && (char.IsUpper(current.Value) ^ char.IsUpper(current.Next.Value)))
        {
            var previous = current.Previous;
            list.Remove(current.Next);
            list.Remove(current);

            current = previous ?? list.First;
        }
        else
        {
            current = current.Next;
        }
    }

    return list;
}