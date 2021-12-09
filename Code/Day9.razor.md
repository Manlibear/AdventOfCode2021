@code
{
    string answer1 = "";
    string answer2 = "";
    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(9).ContinueWith((str) =>
        {
            var lines = str.Result.Split("\r\n");
            List<HeightNode> nodeMap = new List<HeightNode>();
            List<HeightNode> basinBases = new List<HeightNode>();
            List<int> basinSizes = new List<int>();
            int MAX_X = lines[0].Length;
            int MAX_Y = lines.Count();

            // first create a list of nodes based on the input file
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    nodeMap.Add(new HeightNode(x, y, int.Parse(lines[y][x].ToString())));
                }
            }

            // find all the connections that each node has
            foreach (var n in nodeMap)
            {
                n.FindConnections(ref nodeMap, MAX_X, MAX_Y);
            }

            // now do the actual stuff            
            foreach (var n in nodeMap)
            {
                if (n.IsLowest)
                {
                    basinBases.Add(n);
                    int basinSize = 0;
                    n.TraverseBasin(ref basinSize);
                    basinSizes.Add(basinSize);
                }
            }

            answer1 = (basinBases.Select(x => x.Height).Sum() + basinBases.Count()).ToString();
            answer2 = basinSizes.OrderByDescending(x => x).Take(3).Aggregate((a, x) => a * x).ToString();
        });
    }

    public class HeightNode
    {
        List<HeightNode> _connections;
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        bool visited = false;
        public bool IsLowest => _connections.All(n => n.Height > Height);

        public HeightNode(int x, int y, int height)
        {
            _connections = new List<HeightNode>();
            X = x;
            Y = y;
            Height = height;
        }

        public void FindConnections(ref List<HeightNode> allNodes, int maxX, int maxY)
        {
            if (Y > 0)
                _connections.Add(allNodes[X + ((Y - 1) * maxX)]);

            if (Y < (maxY - 1))
                _connections.Add(allNodes[X + ((Y + 1) * maxX)]);

            if (X > 0)
                _connections.Add(allNodes[(X - 1) + (Y * maxX)]);

            if (X < (maxX - 1))
                _connections.Add(allNodes[(X + 1) + (Y * maxX)]);
        }

        public void TraverseBasin(ref int size)
        {
            visited = true;
            size++;
            foreach (var c in _connections.Where(x => !x.visited && x.Height < 9))
            {
                c.TraverseBasin(ref size);
            }
        }
    }
}
