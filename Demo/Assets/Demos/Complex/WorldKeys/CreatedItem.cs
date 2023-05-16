using CrashKonijn.Goap.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.WorldKeys
{
    public class CreatedItem<TCreatable> : WorldKeyBase
        where TCreatable : ICreatable
    {
    }
}