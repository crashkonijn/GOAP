using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.WorldKeys
{
    public class CreatedItem<TCreatable> : WorldKeyBase
        where TCreatable : ICreatable
    {
    }
}