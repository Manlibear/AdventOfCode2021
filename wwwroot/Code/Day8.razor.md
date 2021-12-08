@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(8).ContinueWith((str) =>
        {
            var lines = str.Result.Split("\n").Select(x =>
    {
    var parts = x.Split(" | ");
    return new
    {
        Input = parts[0].Trim().Replace("\n", "").Split(" "),
        Output = parts[1].Trim().Replace("\n", "").Split(" ")
    };
            }).ToDictionary(x => x.Input, x => x.Output);

            Dictionary<int, int> numLookup = new Dictionary<int, int>() { { 2, 1 }, { 3, 7 }, { 4, 4 }, { 7, 8 } };

            int known = 0;
            int sum = 0;

            foreach (var l in lines)
            {
                Display disp = new Display();

                disp.Parse(l.Key);

                string sInt = "";
                foreach (var o in l.Value)
                {
                    if (numLookup.ContainsKey(o.Length))
                        known++;

                    var parsed = disp.Read(o).ToString();

                    @* Console.WriteLine($"{o} -> {parsed}"); *@

                    sInt += parsed;
                }

                Console.WriteLine($"{string.Join(' ', l.Value)} -> {sInt}");
                sum += int.Parse(sInt);
            }

            answer1 = known.ToString();
            answer2 = sum.ToString();
        });
    }

    public class Display
    {
        string[] parsed = new string[10];

        // Determine a known number, mapping the length to winding order
        public void Parse(string[] s)
        {
            var grouped = s.GroupBy(x => x.Length);

            // step 1, get known numbers
            var one = grouped.First(x => x.Key == 2).First();
            var four = grouped.First(x => x.Key == 4).First();
            var seven = grouped.First(x => x.Key == 3).First();
            var eight = grouped.First(x => x.Key == 7).First();

            var sixChars = grouped.First(x => x.Key == 6);
            var fiveChars = grouped.First(x => x.Key == 5);

            // step 2, find 6, it's the only 6 char without all one segs, eight - six = UR
            var six = FindWithoutAll(ref sixChars, one);
            char UpRight = RemoveSegments(six, eight);

            // step 3, find 5, it's the only 5 char without all one segs, six - five = DL
            var five = FindWithoutAll(ref fiveChars, UpRight.ToString());
            char DownLeft = RemoveSegments(five, six);

            // step 4, find 9, it's the only 6 char without DownLeft
            var nine = FindWithoutAll(ref sixChars, DownLeft.ToString());

            // step 5, find 0 is the last of the sixChars
            var zero = sixChars.First();

            // step 6, find 3, it's the only 5 char missing DownLeft
            var three = FindWithoutAll(ref fiveChars, DownLeft.ToString());

            // step 7, find 2, it's the last of the fiveChars
            var two = fiveChars.First();

            parsed = new string[10] { zero, one, two, three, four, five, six, seven, eight, nine };
            @*
                foreach (var p in parsed)
                Console.Write($"{p},");

                Console.WriteLine(); *@
        }

        private char RemoveSegments(string a, string b)
        {
            foreach (var c in a)
                b = b.Replace(c.ToString(), "");

            if (b.Length > 1)
                throw new Exception("Ambiguous segment");

            return b[0];
        }

        private string FindWithoutAll(ref IGrouping<int, string> g, string w)
        {
            foreach (var x in g.Select(s => s))
            {
                foreach (var c in w)
                {
                    if (!x.Contains(c))
                    {
                        g = g.Where(s => s != x).GroupBy(s => s.Length).First();
                        return x;
                    }
                }
            }

            throw new Exception("Unable to find any without all");
        }

        public int Read(string s)
        {
            for (int i = 0; i < parsed.Length; i++)
            {
                if (parsed[i].Length == s.Length)
                {
                    bool valid = true;
                    foreach (var c in s)
                    {
                        if (!parsed[i].Contains(c))
                            valid = false;
                    }

                    if (valid)
                        return i;
                }
            }

            return -1;
        }
    }
}
