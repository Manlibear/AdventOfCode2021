@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(7).ContinueWith((str) =>
        {
            int[] pos = str.Result.Split(",").Select(x => int.Parse(x)).ToArray();

            var grouped = pos.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            Dictionary<int, (int predicted, int actual)> cost = new Dictionary<int, (int predicted, int actual)>();
            var max = grouped.Max(x => x.Key);

            for (int i = 0; i < max; i++)
            {
                int thisCost = 0;
                int thisActualCost = 0;
                foreach (var bg in grouped.OrderBy(x => x.Value))
                {
                    var dist = Math.Abs(bg.Key - i);
                    thisCost += dist * bg.Value;
                    thisActualCost += (((dist * dist) + dist) / 2) * bg.Value; // nth triangle number
                }

                cost[i] = (thisCost, thisActualCost);
            }

            answer1 = cost.Min(x => x.Value.predicted).ToString();
            answer2 = cost.Min(x => x.Value.actual).ToString();

        });
    }
}
