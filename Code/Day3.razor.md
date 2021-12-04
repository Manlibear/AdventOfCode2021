@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        // least common
        string epsilon = "";
        // most common
        string gamma = "";
        var oxygen = "";
        var co2 = "";

        await DayApi.GetDayInput(3).ContinueWith((str) =>
        {
            var lines = str.Result.Split("\n").ToList();
            var linesOxy = str.Result.Split("\n").ToList();
            var linesCo2 = str.Result.Split("\n").ToList();

            var lineLength = lines[0].Length - 1;
            // if there are more zeroes than half of the length of input, then it's the most common
            int halfCount = (lines.Count() / 2);

            for (int c = 0; c < lineLength; c++)
            {
                // count the zeroes in each column, seperately for oxygen and co2 levels
                var z = lines.Count(x => x[c] == '0');
                var zO = linesOxy.Count(x => x[c] == '0');
                var zC = linesCo2.Count(x => x[c] == '0');

                gamma += (z > halfCount ? "0" : "1");
                // epsilon is simply inverse gamma
                epsilon += gamma.Last() == '0' ? "1" : "0";

                // if we still have multiple oxygen values left to filter, pick the most common, otherwise just use the current
                oxygen += linesOxy.Count() > 1 ? (zO > (linesOxy.Count() / 2) ? "0" : "1") : linesOxy[0][c];
                linesOxy = linesOxy.Where(x => x.StartsWith(oxygen)).ToList();

                //... same as above, but differing rules
                co2 += linesCo2.Count() > 1 ? (zC <= (linesCo2.Count() / 2) ? "0" : "1") : linesCo2[0][c];
                linesCo2 = linesCo2.Where(x => x.StartsWith(co2)).ToList();
            }

            answer1 = (Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2)).ToString();
            answer2 = (Convert.ToInt32(oxygen, 2) * Convert.ToInt32(co2, 2)).ToString();
        });
    }
}
