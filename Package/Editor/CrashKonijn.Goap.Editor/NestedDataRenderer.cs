using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CrashKonijn.Goap.Editor
{
    class NestedDataRenderer : TreeView
    {
        private object data;
        private int id = 0;

        public NestedDataRenderer(TreeViewState treeViewState, object data)
            : base(treeViewState)
        {
            this.data = data;
            this.Reload();
        }
        
        protected override TreeViewItem BuildRoot ()
        {
            // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
            // are created from data. Here we create a fixed set of items. In a real world example,
            // a data model should be passed into the TreeView and the items created from the model.

            // This section illustrates that IDs should be unique. The root item is required to 
            // have a depth of -1, and the rest of the items increment from that.
            var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};
            var allItems = new List<TreeViewItem>();
            // var allItems = new List<TreeViewItem> 
            // {
            //     new TreeViewItem {id = 1, depth = 0, displayName = "Animals"},
            //     new TreeViewItem {id = 2, depth = 1, displayName = "Mammals"},
            //     new TreeViewItem {id = 3, depth = 2, displayName = "Tiger"},
            //     new TreeViewItem {id = 4, depth = 2, displayName = "Elephant"},
            //     new TreeViewItem {id = 5, depth = 2, displayName = "Okapi"},
            //     new TreeViewItem {id = 6, depth = 2, displayName = "Armadillo"},
            //     new TreeViewItem {id = 7, depth = 1, displayName = "Reptiles"},
            //     new TreeViewItem {id = 8, depth = 2, displayName = "Crocodile"},
            //     new TreeViewItem {id = 9, depth = 2, displayName = "Lizard"},
            // };

            this.id = 0;
            this.AddProperties(allItems, this.data, 0);

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths (root, allItems);
            
            // Return root of the tree
            return root;
        }

        private void AddProperties(List<TreeViewItem> items, object obj, int depth)
        {
            UnityEngine.Debug.Log($"AddProperties: {obj}");
            if (obj == null)
                return;
            
            var properties = obj.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);

                if (value.GetType().IsClass)
                {
                    items.Add(new TreeViewItem { id = this.id++, depth = depth, displayName = $"{property.Name}"});
                    this.AddProperties(items, value, depth + 1);
                    return;
                }

                items.Add(new TreeViewItem { id = this.id++, depth = depth, displayName = $"{property.Name}: {value}"});
            }
        }

        public void OnGUI(Rect rect, object data)
        {
            UnityEngine.Debug.Log("Draw: " + data);
            this.data = data;
            this.OnGUI(rect);
        }
    }
}