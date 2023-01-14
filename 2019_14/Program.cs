var reactions = File.ReadAllLines("input.txt").Select(line =>
    {
        var sp = line.Split(" => ");
        var arr2 = sp[1].Split(' ');
        var makes = arr2[1];
        var quantity = int.Parse(arr2[0]);

        (string precursor, int quantity)[] inputs = sp[0].Split(", ")
            .Select(input =>
            {
                var arr = input.Split(' ');
                return (arr[1], int.Parse(arr[0]));
            })
        .ToArray();

        return (makes, quantity, inputs);
    })
    .ToDictionary(tp => tp.makes, tp => (tp.quantity, tp.inputs));

var part1 = getOreCost(reactions.Keys.ToDictionary(key => key, _ => 0L), "FUEL", 1);
Console.WriteLine($"Part 1: {part1}");

//Enter required FUEL
//4052920
//Ore cost: 999999813444
//Enter required FUEL
//4052921
//Ore cost: 1000000088939
while (true)
{
    Console.WriteLine($"Enter required FUEL");
    var required = int.Parse(Console.ReadLine());
    Console.WriteLine($"Ore cost: {getOreCost(reactions.Keys.ToDictionary(key => key, _ => 0L), "FUEL", required)}");
}

long getOreCost(Dictionary<string, long> stores, string product, long requires)
{
    if (product == "ORE")
        return requires;

    //if we have any in store for this ingredient then use it
    var tmp = requires;
    requires = Math.Max(0, requires - stores[product]);
    stores[product] -= Math.Min(stores[product], tmp);

    //how many batches do we need to make
    //any excess goes in to stores
    var batches = (long)Math.Ceiling((double)requires / reactions[product].quantity);
    stores[product] += reactions[product].quantity * batches - requires;

    long ore = 0;
    foreach (var input in reactions[product].inputs)
    {
        //recursively get the cost of all the required inputs
        ore += getOreCost(stores, input.precursor, input.quantity * batches);
    }

    return ore;
}