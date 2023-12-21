using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;

namespace CrashKonijn.Goap.Demos.Complex.Classes.Items
{
    public class Apple : ItemBase, IEatable
    {
        public float NutritionValue { get; set; } = 200f;
    }
}