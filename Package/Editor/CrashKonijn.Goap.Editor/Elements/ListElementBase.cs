using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public abstract class ListElementBase<TItemType, TRenderType> : VisualElement
        where TItemType : new()
        where TRenderType : VisualElement, IFoldable
    {
        private readonly List<TItemType> list;
        private TItemType selectedItem;
        private readonly VisualElement elementsRoot;
        private VisualElement buttonContainer;

        public ListElementBase(List<TItemType> list)
        {
            this.list = list;
            this.elementsRoot = new VisualElement();
            this.Add(this.elementsRoot);
        }

        public void Rebuild()
        {
            this.elementsRoot.Clear();
            
            for (var i = 0; i < this.list.Count; i++)
            {
                this.CreateElement(i);
            }

            if (this.buttonContainer is not null)
                return;
            
            this.buttonContainer = new VisualElement();
            this.buttonContainer.style.flexDirection = FlexDirection.Row;
            this.buttonContainer.style.justifyContent = Justify.FlexEnd;

            var addButton = new Button(this.AddItem)
            {
                text = "+"
            };
            this.buttonContainer.Add(addButton);

            var removeButton = new Button(this.RemoveSelectedItem)
            {
                text = "-"
            };
            this.buttonContainer.Add(removeButton);

            this.Add(this.buttonContainer);
        }

        private void CreateElement(int i)
        {
            var element = this.CreateListItem(this.list[i]);
            element.RegisterCallback<ClickEvent>(evt =>
            {
                this.selectedItem = this.list[i];
            });
            this.elementsRoot.Add(element);
                
            this.BindListItem(element, this.list[i], i);
        }

        protected abstract TRenderType CreateListItem(TItemType item);

        private void BindListItem(VisualElement element, int index)
        {
            this.BindListItem(element as TRenderType, this.list[index], index);
        }
        
        protected abstract void BindListItem(TRenderType element, TItemType item, int index);
        
        private void AddItem()
        {
            // Add your item to the scriptable's list
            // Example: scriptable.items.Add(newItem);
            this.list.Add(new TItemType());

            this.CreateElement(this.list.Count -1);

            this.CloseAll();
            
            this.GetFoldable(this.list.Count - 1).Foldout.value = true;
            
            // this.Rebuild(); // Refresh the ListView
        }

        private void RemoveSelectedItem()
        {
            this.list.Remove(this.selectedItem);
            
            this.Rebuild();
        }

        private void CloseAll()
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                this.GetFoldable(i).Foldout.value = false;
            }
        }
        
        private IFoldable GetFoldable(int index)
        {
            return this.elementsRoot.ElementAt(index) as IFoldable;
        }
    }

    public interface IFoldable
    {
        public Foldout Foldout { get; }
    }
}