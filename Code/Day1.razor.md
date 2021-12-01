@code
{
    string answer = "";
    async void Anwser()
    {
        await DayApi.GetDayInput(1).ContinueWith((str) =>
        {
            int? prev = null;
            int larger = 0;
            foreach (var l in str.Result.Split("\n"))
            {
                int x = int.Parse(l);
                if (prev.HasValue && prev.Value < x)
                {
                    larger++;
                }
                prev = x;
            }

            answer = larger.ToString();
        });
    }
}
