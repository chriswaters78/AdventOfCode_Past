var results = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}").Select(str => str.Replace(Environment.NewLine, " ").Split(' ')).ToArray();
var valid = results.Where(arr => arr.Length == 8 || (arr.Length == 7 && !arr.Any(str => str.Contains("cid"))));
var results2 = valid.Select(arr => arr.Select(str => str.Split(":")).ToArray()).ToArray();

var part1 = valid.Count();
var part2 = results2.Where(arr => arr.All(arr2 => isValid(arr2[0], arr2[1])));

Console.WriteLine(part1);
Console.WriteLine(part2.Count());

bool isValid(string key, string value)
{
    switch (key)
    {
        case "byr":
            int byear;
            if (!int.TryParse(value, out byear))
            {
                return false;
            }
            return byear >= 1920 && byear <= 2002;
        case "iyr":
            int iyear;
            if (!int.TryParse(value, out iyear))
            {
                return false;
            }
            return iyear >= 2010 && iyear <= 2020;
        case "eyr":
            int eyear;
            if (!int.TryParse(value, out eyear))
            {
                return false;
            }
            return eyear >= 2020 && eyear <= 2030;
        case "hgt":
            if (!value.EndsWith("in") && !value.EndsWith("cm"))
            {
                return false;
            }
            int height;
            if (!int.TryParse(value.Substring(0, value.Length - 2), out height))
            {
                return false;
            }
            if (value.EndsWith("in"))
            {
                return height >= 59 && height <= 76;
            }
            else
            {
                return height >= 150 && height <= 193;
            }
        case "hcl":
            if (value.Length != 7)
            {
                return false;
            }
            if (!value.Skip(1).All(ch => Char.IsDigit(ch) || Char.IsLower(ch)))
            {
                return false;
            }
            return true;
        case "ecl":
            return new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(value);
        case "pid":
            return value.Length == 9 && value.All(ch => Char.IsDigit(ch));
        case "cid":
            return true;
        default:
            throw new Exception();
    }
}