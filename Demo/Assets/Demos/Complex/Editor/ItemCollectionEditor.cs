using Demos.Complex.Behaviours;
using UnityEditor;
using UnityEngine.UIElements;

namespace Demos.Complex.Editor
{
    [CustomEditor(typeof(ItemCollection))]
    public class ItemCollectionEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var collection = (ItemCollection) this.target;
            
            var root = new VisualElement();

            root.Add(new Label("Items"));

            var box = new Box();
            
            foreach (var holdable in collection.All())
            {
                box.Add(new Label(holdable.GetType().Name));
            }
            
            root.Add(box);
            
            return root;
        }
    }
}