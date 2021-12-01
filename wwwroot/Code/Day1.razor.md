@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(1).ContinueWith((str) =>
        {
            int larger1 = 0, larger2 = 0;

            var lines = str.Result.Split("\n").Select(x => int.Parse(x)).ToList();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (i > 0 && lines[i - 1] < lines[i])
                    larger1++;

                if (i > 2)
                {
                    var thisGroup = lines.Skip(i - 2).Take(3).Sum();
                    var lastGroup = lines.Skip(i - 3).Take(3).Sum();

                    if (thisGroup > lastGroup)
                        larger2++;
                }
            }

            answer1 = larger1.ToString();
            answer2 = larger2.ToString();
        });
    }
}
