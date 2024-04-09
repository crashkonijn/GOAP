using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
    public class ToolbarElement : Toolbar
    {
        public ToolbarElement(SelectedObject selectedObject, EditorWindowValues values)
        {
            this.Add(new ToolbarButton(() =>
            {
                Selection.activeObject = selectedObject.Object;
            })
            {
                text = selectedObject.Object.name
            });
            
            this.Add(new ToolbarButton(() =>
            {
                var elementsWithClass = values.RootElement.Query<VisualElement>(className: "node").ToList();

                foreach (var element in elementsWithClass)
                {
                    element.AddToClassList("collapsed");
                }
            })
            {
                text = "collapse"
            });
            
            this.Add(new ToolbarButton(() =>
            {
                var elementsWithClass = values.RootElement.Query<VisualElement>(className: "node").ToList();

                foreach (var element in elementsWithClass)
                {
                    element.RemoveFromClassList("collapsed");
                }
            })
            {
                text = "open"
            });
            
            var spacer = new VisualElement();
            spacer.style.flexGrow = 1; // This makes the spacer flexible, filling available space
            this.Add(spacer);
            
            this.Add(new ToolbarButton(() =>
            {
                values.UpdateZoom(10);
            })
            {
                text = "+"
            });
            this.Add(new ToolbarButton(() =>
            {
                values.UpdateZoom(-10);
            })
            {
                text = "-"
            });
            this.Add(new ToolbarButton(() =>
            {
                values.Zoom = 100;
                values.DragDrawer.Reset();
            })
            {
                text = "reset"
            });
        }
    }
}