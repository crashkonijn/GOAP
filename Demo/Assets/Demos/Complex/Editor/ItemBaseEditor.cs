using Demos.Complex.Behaviours;
using UnityEditor;
using UnityEngine.UIElements;

namespace Demos.Complex.Editor
{
    [CustomEditor(typeof(ItemBase))]
    public class ItemBaseEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var item = (ItemBase) this.target;
            
            var root = new VisualElement();

            var box = new Box();
            
            box.Add(new Label($"Is Held: {item.IsHeld}"));
            box.Add(new Label($"Is In Box: {item.IsInBox}"));
            box.Add(new Label($"Is Claimed: {item.IsClaimed}"));
            
            root.Add(box);

            return root;
        }
    }
}