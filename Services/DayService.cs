namespace AdventOfCodeBlazor.Services
{
    public interface IDayApi
    {
        bool HasCompletedDay(int day);
    }

    public class DayApi : IDayApi
    {
        private readonly int[] _completedDays = new int[]{
                1
            };

        public bool HasCompletedDay(int day) => _completedDays.Contains(day);
    }
}