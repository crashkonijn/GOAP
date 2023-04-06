using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Classes.Items
{
    public class Pickaxe : ItemBase, ICreatable
    {
        public int BuildPoints { get; set; } = 200;
    }
}