
using Microsoft.AspNetCore.Components;

namespace AdventOfCodeBlazor.Services
{
    public interface IDayApi
    {
        bool HasCompletedDay(int day);
        Task<string> GetDayInput(int day);
    }

    public class DayApi : IDayApi
    {
        public DayApi(HttpClient http)
        {
            Http = http;
        }

        [Inject]
        private HttpClient Http { get; set; }
        private readonly int[] _completedDays = new int[]{
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
            };

        public async Task<string> GetDayInput(int day)
        {
            return await Http.GetStringAsync($"Input/Day{day}.input");
        }

        public bool HasCompletedDay(int day) => _completedDays.Contains(day);
    }
}