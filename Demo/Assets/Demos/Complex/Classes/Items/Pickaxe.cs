using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Classes.Items
{
    public class Pickaxe : ItemBase, ICreatable
    {
        public bool IsHeld { get; set; }
        public int BuildPoints { get; set; } = 200;
    }
}