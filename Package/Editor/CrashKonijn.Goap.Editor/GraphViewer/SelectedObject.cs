using UnityEngine;

namespace CrashKonijn.Goap.Editor.GraphViewer
{
    public class SelectedObject
    {
        public Object Object { get; private set; }
        
        public void SetObject(Object obj)
        {
            this.Object = obj;
        }
    }
}