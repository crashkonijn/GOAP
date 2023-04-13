using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Classes.Items
{
    public class Axe : ItemBase, ICreatable
    {
        public int BuildPoints { get; set; } = 100;
    }
}