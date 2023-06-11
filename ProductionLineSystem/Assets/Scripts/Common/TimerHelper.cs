using System.Timers;

namespace Common
{
    public static class TimerHelper
    {
        public static void Reset(this Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

    }
}
