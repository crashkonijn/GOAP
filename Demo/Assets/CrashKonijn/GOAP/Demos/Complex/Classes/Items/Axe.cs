using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Classes.Items
{
    public class Axe : ItemBase, ICreatable
    {
        public int BuildPoints { get; set; } = 100;
    }
}