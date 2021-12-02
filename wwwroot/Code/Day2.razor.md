@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(2).ContinueWith((str) =>
        {
            int x = 0, y = 0;

            foreach (var l in str.Result.Split("\n"))
            {
                var parts = l.Split(" ");
                var value = int.Parse(parts[1]);

                switch (parts[0])
                {
                    case "forward":
                        x += value;
                        break;

                    case "up":
                        y -= value;
                        break;

                    case "down":
                        y += value;
                        break;
                }
            }

            answer1 = x * y;
        });
    }
}
