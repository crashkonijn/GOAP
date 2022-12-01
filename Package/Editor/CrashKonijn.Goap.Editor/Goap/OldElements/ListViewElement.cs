using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Goap.OldElements
{
    public class ListViewElement<T> : IEditorElement
        where T : class, IConfig
    {
        private readonly List<T> elements;
        private readonly Action<T> onSelectionChange;

        public ListViewElement(List<T> elements, Action<T> onSelectionChange)
        {
            this.elements = elements;
            this.onSelectionChange = onSelectionChange;
        }
        
        public VisualElement Render(EditorState state, Action onChange)
        {
            var list = new ListView();
            
            list.makeItem = () => new Label();
            list.bindItem = (item, index) => { (item as Label).text = this.elements[index].Name; };
            list.itemsSource = this.elements;

            list.onSelectionChange += _ =>
            {
                this.onSelectionChange(list.selectedItem as T);
            };

            return list;
        }
    }
}