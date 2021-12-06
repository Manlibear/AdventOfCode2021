@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(6).ContinueWith((str) =>
        {
            int[] fish = str.Result.Split(",").Select(x => int.Parse(x)).ToArray();

            long[] fishAges = new long[9];

            foreach (var f in fish)
                fishAges[f]++;

            for (int i = 0; i < 256; i++)
            {
                var newFish = fishAges[0];
                var shifted = new long[9];

                Array.Copy(fishAges, 1, shifted, 0, shifted.Length - 1);
                shifted[8] = newFish;
                shifted[6] += newFish;

                fishAges = shifted;

                if (i == 79) // 80th day, part 1 answer
                    answer1 = fishAges.Sum().ToString();
            }

            answer2 = fishAges.Sum().ToString();

        });
    }
}
