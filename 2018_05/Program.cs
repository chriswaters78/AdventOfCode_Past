// See https://aka.ms/new-console-template for more information
var input = new LinkedList<char>(File.ReadAllText("test.txt"));

var current = input.First;
while (current.Next != null)
{
    if ((char.IsLower(current.Value) && char.ToUpper(current.Value) == current.Next.Value)
        || (char.IsUpper(current.Value) && char.ToLower(current.Value) == current.Next.Value))
    {
        input.Remove(current.Next);
        input.Remove(current);
        current = input.First;
    }
    else
    {
        current = current.Next;
    }
}

Console.WriteLine($"Part 1: {input.Count}");
