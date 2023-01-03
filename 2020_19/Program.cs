class Program
{
    public static void Main(string[] args)
    {
        var lines = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
        var samples = lines[1].Split($"{Environment.NewLine}");

        var rules = new Dictionary<string, string[][]>();
        foreach (var line in lines[0].Split($"{Environment.NewLine}"))
        {
            var sp = line.Split(": ");

            if (sp[1].StartsWith('"'))
            {
                rules.Add(sp[0], new[] { new[] { sp[1][1..^1] } });
            }
            else
            {
                rules.Add(sp[0], sp[1].Split('|').Select(str => str.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray());
            }
        }

        var results1 = samples.Where(sample => match(rules, "0", 0, sample).Any(str => str.Length == 0)).ToList();
        var part1 = results1.Count();

        rules["8"] = new[] { 
            new[] { "42" }, 
            new[] { "42", "8" } 
        };
        
        rules["11"] = new[] { 
            new[] { "42", "31" }, 
            new[] { "42", "11", "31" } 
        };

        var results2 = samples.Where(sample => match(rules, "0", 0, sample).Any( str => str.Length == 0)).ToList();
        var part2 = results2.Count();
    }

    static List<string>  match(Dictionary<string, string[][]> rules, string ruleKey, int ruleIndex, string remainder)
    {
        if (ruleIndex >= rules[ruleKey].Length) return new ();

        var rule = rules[ruleKey][ruleIndex];
        return  (!Char.IsDigit(rule[0][0])) ? 
                    remainder.StartsWith(rule.First()[0]) ? 
                        new () { remainder[rule[0].Length..] } : new ()
                    :   rule.Aggregate(
                            Enumerable.Repeat(remainder,1), 
                            (acc, subRuleKey) => 
                                acc.SelectMany(validRemainer => match(rules, subRuleKey, 0, validRemainer)))
                        .Concat(match(rules, ruleKey, ruleIndex + 1, remainder))
                        .Distinct().ToList();
    }
}