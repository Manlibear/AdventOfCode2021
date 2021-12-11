@code
{
    string answer1 = "";
    string answer2 = "";
    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(10).ContinueWith((str) =>
        {
            var lines = str.Result.Split("\r\n");

            Dictionary<char, (Chunk.ChunkType Type, bool IsOpening)> syntaxLookup =
                new Dictionary<char, (Chunk.ChunkType Type, bool IsOpening)>()
                {
                    {'[', (Chunk.ChunkType.SquareBracket, true)},
                    {'{', (Chunk.ChunkType.Brace, true)},
                    {'<', (Chunk.ChunkType.AngleBracket, true)},
                    {'(', (Chunk.ChunkType.Parenthesis, true)},

                    {']', (Chunk.ChunkType.SquareBracket, false)},
                    {'}', (Chunk.ChunkType.Brace, false)},
                    {'>', (Chunk.ChunkType.AngleBracket, false)},
                    {')', (Chunk.ChunkType.Parenthesis, false)},
                };

            List<Chunk> chunks = new List<Chunk>();

            Dictionary<Chunk.ChunkType, int> invalidChars = new Dictionary<Chunk.ChunkType, int>()
            {
                {Chunk.ChunkType.AngleBracket, 0},
                {Chunk.ChunkType.Brace, 0},
                {Chunk.ChunkType.Parenthesis, 0},
                {Chunk.ChunkType.SquareBracket, 0},
            };

            List<string> lineCompletion = new List<string>();

            int i = 0;
            foreach (var l in lines)
            {
                bool invalid = false;
                i++;
                foreach (var c in l)
                {
                    var found = syntaxLookup[c];
                    if (found.IsOpening)
                    {
                        chunks.Add(new Chunk(found.Type));
                    }
                    else
                    {
                        for (int x = chunks.Count() - 1; x >= 0; x--)
                        {
                            if (!chunks[x].Closed)
                            {
                                if (chunks[x].Type == found.Type)
                                {
                                    chunks[x].Closed = true;
                                    break;
                                }
                                else
                                {
                                    invalidChars[found.Type]++;
                                    invalid = true;
                                    break;
                                }
                            }
                        }

                        if (invalid)
                            break;
                    }
                }

                if (!invalid)
                {
                    while (chunks.Any(x => !x.Closed))
                    {
                        string autoComplete = "";
                        for (int p = chunks.Count() - 1; p >= 0; p--)
                        {
                            if (!chunks[p].Closed)
                            {
                                autoComplete += syntaxLookup.First(x => x.Value.Type == chunks[p].Type && !x.Value.IsOpening).Key;
                                chunks[p].Closed = true;
                            }

                        }
                        lineCompletion.Add(autoComplete);
                    }
                }

                chunks.Clear();
            }


            answer1 = (
                invalidChars[Chunk.ChunkType.Parenthesis] * 3 +
                invalidChars[Chunk.ChunkType.SquareBracket] * 57 +
                invalidChars[Chunk.ChunkType.Brace] * 1197 +
                invalidChars[Chunk.ChunkType.AngleBracket] * 25137
            ).ToString();

            List<long> autoCompleteScores = new List<long>();
            foreach (var l in lineCompletion)
            {
                long thisLineScore = 0;
                foreach (var c in l)
                {
                    thisLineScore *= 5;
                    switch (c)
                    {
                        case ')':
                            thisLineScore += 1;
                            break;
                        case ']':
                            thisLineScore += 2;
                            break;
                        case '}':
                            thisLineScore += 3;
                            break;
                        case '>':
                            thisLineScore += 4;
                            break;
                    }
                }

                autoCompleteScores.Add(thisLineScore);
                Console.WriteLine(thisLineScore);
            }

            answer2 = autoCompleteScores.OrderBy(x => x).Skip(autoCompleteScores.Count() / 2).Take(1).First().ToString();

        });
    }

    public class Chunk
    {
        public bool Closed { get; set; } = false;
        public ChunkType Type { get; set; }

        public Chunk(ChunkType type)
        {
            Type = type;
        }

        public enum ChunkType
        {
            Brace,
            Parenthesis,
            SquareBracket,
            AngleBracket
        }
    }
}
