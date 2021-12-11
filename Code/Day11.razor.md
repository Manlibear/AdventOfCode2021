@code
{
    string answer1 = "";
    string answer2 = "";
    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(11).ContinueWith((str) =>
        {
            var lines = str.Result.Split("\r\n");

            List<Octopus> octopi = new List<Octopus>();
            int MAX_X = lines[0].Length;
            int MAX_Y = lines.Count();
            int STEPS = 100;

            for (int y = 0; y < lines.Length; y++)
                for (int x = 0; x < lines[y].Length; x++)
                    octopi.Add(new Octopus(x, y, lines[y][x] - '0'));

            foreach (var o in octopi)
                o.FindNieghbours(octopi, MAX_X, MAX_Y);

            int totalFlashes = 0;
            int i = 0;

            while (true)
            {
                i++;
                // first step is reset everyone's Flashed state
                octopi.ForEach(x => x.Flashed = false);

                // up everyone's energy by 1
                octopi.ForEach(x => x.Energy++);

                // handle flashes
                octopi.ForEach(x => x.Flash(ref totalFlashes));

                if (i == STEPS)
                    answer1 = totalFlashes.ToString();

                // check if all have flashed
                if (octopi.All(x => x.Flashed))
                {
                    answer2 = i.ToString();
                    break;
                }
            }
        });
    }

    public class Octopus
    {
        public List<Octopus> Neighbours { get; set; }
        public bool Flashed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Energy { get; set; }

        public Octopus(int x, int y, int energy)
        {
            X = x;
            Y = y;
            Energy = energy;
            Neighbours = new List<Octopus>();
        }

        public void Flash(ref int totalFlashes)
        {
            if (Energy > 9)
            {
                Flashed = true;
                Energy = 0;
                totalFlashes++;

                foreach (var o in Neighbours.Where(x => !x.Flashed))
                {
                    o.Energy++;
                    o.Flash(ref totalFlashes);
                }
            }
        }

        public void FindNieghbours(List<Octopus> allOctopi, int maxX, int maxY)
        {
            if (Y > 0)
            {
                Neighbours.Add(allOctopi[X + ((Y - 1) * maxX)]);

                if (X > 0)
                    Neighbours.Add(allOctopi[(X - 1) + ((Y - 1) * maxX)]);

                if (X < (maxX - 1))
                    Neighbours.Add(allOctopi[(X + 1) + ((Y - 1) * maxX)]);

            }

            if (Y < (maxY - 1))
            {
                Neighbours.Add(allOctopi[X + ((Y + 1) * maxX)]);

                if (X > 0)
                    Neighbours.Add(allOctopi[(X - 1) + ((Y + 1) * maxX)]);

                if (X < (maxX - 1))
                    Neighbours.Add(allOctopi[(X + 1) + ((Y + 1) * maxX)]);
            }

            if (X > 0)
                Neighbours.Add(allOctopi[(X - 1) + (Y * maxX)]);

            if (X < (maxX - 1))
                Neighbours.Add(allOctopi[(X + 1) + (Y * maxX)]);
        }
    }
}
