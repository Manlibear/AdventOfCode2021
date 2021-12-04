@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(4).ContinueWith((str) =>
        {
            var callingNumbers = str.Result.Split("\n").First().Split(",");

            var boards = new List<Board>();
            var boardInputs = str.Result.Split("\r\n\r\n").Skip(1).ToList(); // skip one for the calling row

            foreach (var b in boardInputs)
            {
                boards.Add(new Board(b));
            }

            List<Board> winningBoards = new List<Board>();

            foreach (var n in callingNumbers)
            {
                var number = int.Parse(n);

                foreach (var b in boards.Where(x => x.Score == 0))
                {
                    b.CallNumber(number);
                    b.Score = b.CalculateScore() * number;

                    if (b.Score != 0)
                    {
                        winningBoards.Add(b);
                    }
                }
            }

            answer1 = winningBoards.First().Score.ToString();
            answer2 = winningBoards.Last().Score.ToString();

        });
    }

    class Board
    {
        public Board(string input)
        {
            var rows = input.Split("\r\n");
            Tiles = new Tile[rows.Length][];

            for (int i = 0; i < rows.Length; i++)
            {
                var cells = rows[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                Tiles[i] = new Tile[cells.Length];

                for (int j = 0; j < cells.Length; j++)
                {
                    Tiles[i][j] = new Tile(cells[j], j, i);
                }
            }
        }

        public void CallNumber(int number)
        {
            foreach (var r in Tiles)
            {
                foreach (var c in r)
                {
                    if (c.Value == number)
                        c.Found = true;
                }
            }
        }

        public int CalculateScore()
        {
            var allTiles = Tiles.SelectMany(x => x.Where(t => t.Found).Select(y => y)).ToList();

            var foundRows = allTiles.GroupBy(x => x.IndexX);
            var foundColumns = allTiles.GroupBy(x => x.IndexY);

            var completeRow = foundRows.FirstOrDefault(x => x.Count() == Tiles[0].Count());
            var completeCol = foundColumns.FirstOrDefault(x => x.Count() == Tiles.Count());

            if (completeRow != null || completeCol != null)
            {
                return Tiles.SelectMany(x => x.Where(t => !t.Found)).Select(x => x.Value).Sum();
            }

            return 0;
        }

        public int Score = 0;

        Tile[][] Tiles;

        class Tile
        {
            public int Value;
            public bool Found;

            public int IndexY;

            public int IndexX;

            public Tile(int val, int idxX, int idxY, bool found = false)
            {
                Value = val;
                Found = found;
                IndexX = idxX;
                IndexY = idxY;
            }

            public Tile(string val, int idxX, int idxY, bool found = false)
            {
                Value = int.Parse(val);
                Found = found;
                IndexX = idxX;
                IndexY = idxY;
            }
        }
    }
}
