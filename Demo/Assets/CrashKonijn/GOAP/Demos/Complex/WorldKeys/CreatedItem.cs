using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.WorldKeys
{
    public class CreatedItem<TCreatable> : WorldKeyBase
        where TCreatable : ICreatable
    {
    }
}