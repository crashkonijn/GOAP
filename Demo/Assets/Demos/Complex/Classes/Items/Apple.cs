using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Classes.Items
{
    public class Apple : ItemBase, IEatable
    {
        public float NutritionValue { get; set; } = 200f;
    }
}