@code
{
    string answer1 = "";
    string answer2 = "";

    protected override async Task OnInitializedAsync()
    {
        await DayApi.GetDayInput(1).ContinueWith((str) =>
        {
            int larger1 = 0;
            int larger2 = 0;

            var lines = str.Result.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                int x = int.Parse(lines[i]);
                if (i > 0 && int.Parse(lines[i - 1]) > x)
                {
                    larger1++;
                }

                if (i > 2)
                {
                    var thisGroup = int.Parse(lines[i]) + int.Parse(lines[i - 1]) + int.Parse(lines[i - 2]);
                    var lastGroup = int.Parse(lines[i - 1]) + int.Parse(lines[i - 2]) + int.Parse(lines[i - 3]);

                    if (thisGroup > lastGroup)
                        larger2++;
                }
            }
            
            answer1 = larger1.ToString();
            answer2 = larger2.ToString();
        });
    }
}