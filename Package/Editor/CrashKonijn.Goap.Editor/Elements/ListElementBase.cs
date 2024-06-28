using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public abstract class ListElementBase<TItemType, TRenderType> : VisualElement
        where TItemType : class, new()
        where TRenderType : VisualElement, IFoldable
    {
        protected readonly SerializedProperty property;
        private readonly List<TItemType> list;
        private int selectedItemIndex;
        private readonly VisualElement elementsRoot;
        private VisualElement buttonContainer;

        public ListElementBase(SerializedProperty property, List<TItemType> list)
        {
            this.property = property;
            this.list = list;
            this.elementsRoot = new VisualElement();
            this.Add(this.elementsRoot);
        }

        public void Rebuild()
        {
            this.elementsRoot.Clear();

            if (!this.property.isArray)
            {
                return;
            }
            
            for (var i = 0; i < this.property.arraySize; i++)
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
            var value = this.list[i];
            var prop = this.property.GetArrayElementAtIndex(i);
            
            var element = this.CreateListItem(prop, value);
            element.RegisterCallback<ClickEvent>(evt =>
            {
                this.selectedItemIndex = i;
            });
            this.elementsRoot.Add(element);
                
            this.BindListItem(prop, element, value, i);
        }

        protected abstract TRenderType CreateListItem(SerializedProperty property, TItemType item);

        private void BindListItem(SerializedProperty property, VisualElement element, int index)
        {
            this.BindListItem(property, element as TRenderType, this.list[index], index);
        }
        
        protected abstract void BindListItem(SerializedProperty property, TRenderType element, TItemType item, int index);
        
        private void AddItem()
        {
            // Add your item to the scriptable's list
            // Example: scriptable.items.Add(newItem);
            this.property.arraySize++;
            
            // var element = this.property.GetArrayElementAtIndex(this.property.arraySize -1);
            // element.managedReferenceValue = new TItemType();
            
            this.list.Add(new TItemType());
            
            this.CreateElement(this.property.arraySize -1);

            this.CloseAll();
            
            this.GetFoldable(this.property.arraySize -1).Foldout.value = true;
            
            // this.Rebuild(); // Refresh the ListView
        }

        private void RemoveSelectedItem()
        {
            this.list.RemoveAt(this.selectedItemIndex);
            
            this.Rebuild();
        }

        private void CloseAll()
        {
            for (int i = 0; i < this.property.arraySize; i++)
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