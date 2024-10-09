using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public static class SensorTimer
    {
        public static AlwaysSensorTimer Always { get; } = new();
        public static OnceSensorTimer Once { get; } = new();
        public static IntervalSensorTimer Interval(float interval) => new(interval);
    }

    public class AlwaysSensorTimer : ISensorTimer
    {
        public bool ShouldSense(ITimer timer)
        {
            return true;
        }
    }

    public class OnceSensorTimer : ISensorTimer
    {
        public bool ShouldSense(ITimer timer)
        {
            if (timer == null)
                return true;

            return false;
        }
    }

    public class IntervalSensorTimer : ISensorTimer
    {
        private readonly float interval;

        public IntervalSensorTimer(float interval)
        {
            this.interval = interval;
        }

        public bool ShouldSense(ITimer timer)
        {
            if (timer == null)
                return true;

            if (timer.GetElapsed() >= this.interval)
                return true;

            return false;
        }
    }
}
