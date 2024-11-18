namespace CrashKonijn.Goap.Core
{
    public struct SenseValue
    {
        private readonly int value;

        public SenseValue(int value)
        {
            this.value = value;
        }

        public SenseValue(bool value)
        {
            this.value = value ? 1 : 0;
        }

        public static implicit operator int(SenseValue senseValue) => senseValue.value;

        public static implicit operator SenseValue(int value) => new(value);
        public static implicit operator SenseValue(bool value) => new(value);
    }
}
