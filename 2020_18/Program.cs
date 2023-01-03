class Program
{
    public static void Main(string[] args)
    {
        var expression = File.ReadAllLines("input.txt").Select(str => str.Replace(" ", "")).ToArray();
        var part1 = expression.Select(str => calculateExpression(str).acc).Sum();
        var part2 = expression.Select(str => calculateExpression(insertBrackets(str)).acc).Sum();

        Console.WriteLine($"Part1: {part1}, Part2: {part2}");
    }

    private static (long acc, int charsConsumed) calculateExpression(string expression)
    {
        long acc = 0;
        char? lastOp = null;
        for (int c = 0; c < expression.Length; c++)
        {
            if (expression[c] == '(')
            {
                var (result, consumed) = calculateExpression(expression[(c+1)..]);
                c += consumed;
                //will return once we have hit the equivalent close bracket
                //continue on accumulating terms until we hit an ) or the end
                switch (lastOp)
                {
                    case '+':
                        acc += result;
                        break;
                    case '*':
                        acc *= result;
                        break;
                    case null:
                        acc = result;
                        break;
                }
                lastOp = null;
            }
            else if (Char.IsDigit(expression[c]))
            {
                switch (lastOp)
                {
                    case '+':
                        acc += int.Parse(expression[c].ToString());
                        break;
                    case '*':
                        acc *= int.Parse(expression[c].ToString());
                        break;
                    case null:
                        acc = int.Parse(expression[c].ToString());
                        break;
                }
                lastOp = null;
            }
            else if (expression[c] == ')')
            {
                return (acc, c + 1);
            }
            else
            {
                //must be an op
                lastOp = expression[c];
            }
        }

        return (acc, expression.Length);
    }

    private static string insertBrackets(string expression)
    {
        // N+N => (N+N)
        // (expr)+N => ((expr) + N)
        // N+(expr) => (N+(expr))
        for (int c = 0; c < expression.Length; c++)
        {
            if (expression[c] == '+')
            {
                Stack<int> right = new Stack<int>();
                for (int r = c + 1; r < expression.Length; r++)
                {
                    if (expression[r] == '(')
                    {
                        right.Push(r);
                    }
                    else if (expression[r] == ')')
                    {
                        right.Pop();
                    }

                    if (!right.Any())
                    {
                        expression = expression.Insert(r+1, ")");
                        break;
                    }
                }

                Stack<int> left = new Stack<int>();
                for (int l = c - 1; l >= 0; l--)
                {
                    if (expression[l] == ')')
                    {
                        left.Push(l);
                    }
                    else if (expression[l] == '(')
                    {
                        left.Pop();
                    }

                    if (!left.Any())
                    {
                        expression = expression.Insert(l, "(");
                        break;
                    }
                }
                c++;
            }
        }

        return expression;
    }
}