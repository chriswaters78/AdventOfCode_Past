var map = File.ReadAllLines("input.txt").Select(line => line.Split(')')).GroupBy(arr => arr[0])
    .ToDictionary(grp => grp.Key, grp => grp.Select(arr => arr[1]));

var depths = new Dictionary<string, int>();

List<string> pSAN = null;
List<string> pYOU = null;

dfs(0, "COM");
var part1 = depths.Values.Sum();
Console.WriteLine($"Part 1: {part1}");

dfs2(new List<string>() { "COM" });

var path  = pSAN.Zip(pYOU).TakeWhile(tp => tp.First == tp.Second);

var depthCommon = path.Count() - 1;
var part2 = (depths["SAN"] - depthCommon) + (depths["YOU"] - depthCommon) - 2;
Console.WriteLine($"Part 2: {part2}");


void dfs2(List<string> path)
{
    if (path.Last() == "YOU")
    {
        pYOU = path;
    }
    if (path.Last() == "SAN")
    {
        pSAN = path;
    }
    if (!map.ContainsKey(path.Last())) return;
    foreach (var edge in map[path.Last()])
    {
        dfs2(path.Append(edge).ToList());
    }
}


void dfs(int depth, string node)
{
    depths[node] = depth;
    if (!map.ContainsKey(node)) return;
    foreach (var edge in map[node])
    {
        dfs(depth + 1, edge);
    }
}
