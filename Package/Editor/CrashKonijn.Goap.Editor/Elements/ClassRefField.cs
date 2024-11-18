using System;
using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_2021
using UnityEditor.UIElements;
#endif

namespace CrashKonijn.Goap.Editor
{
    public class ClassRefField : VisualElement
    {
        public PopupField<Script> SelectField { get; set; }
        public TextField NameField { get; set; }
        public Circle Status { get; set; }
        
        public ClassRefField()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            
            this.Status = new Circle(Color.black, 10);
            this.Status.style.marginLeft = new StyleLength(new Length(5, LengthUnit.Pixel));
            this.Status.style.marginRight = new StyleLength(new Length(2, LengthUnit.Pixel));
            this.Status.style.marginTop = new StyleLength(new Length(5, LengthUnit.Pixel));
            row.Add(this.Status);
            
            this.NameField = new TextField();
            this.NameField.style.flexGrow = 1;
            
            row.Add(this.NameField);

            this.SelectField = new PopupField<Script>();
            this.SelectField.formatListItemCallback = item => item.Type.Name;
            this.SelectField.style.width = new StyleLength(new Length(20, LengthUnit.Pixel));
            row.Add(this.SelectField);
            
            this.Add(row);
        }

        public void Bind(ScriptableObject scriptable, ClassRef classRef, Script[] scripts, Action<ClassRef> onValueChanged)
        {
            this.NameField.value = classRef.Name; // Replace with the actual property
            this.NameField.RegisterValueChangedCallback(evt =>
            {
                classRef.Name = evt.newValue;
                classRef.Id = "";
                onValueChanged(classRef);
                this.UpdateStatus(classRef, scripts);
                EditorUtility.SetDirty(scriptable); // Mark the scriptable object as dirty
            });

            // element.SelectField.value = item.name;
            this.SelectField.choices = scripts.ToList();
            this.SelectField.RegisterValueChangedCallback(evt =>
            {
                this.NameField.SetValueWithoutNotify(evt.newValue.Type.Name);
                classRef.Name = evt.newValue.Type.Name;
                classRef.Id = evt.newValue.Id;
                this.SelectField.SetValueWithoutNotify(null);
                onValueChanged(classRef);
                this.UpdateStatus(classRef, scripts);
                EditorUtility.SetDirty(scriptable); // Mark the scriptable object as dirty
            });
            
            this.UpdateStatus(classRef, scripts);
            
            this.schedule.Execute(() =>
            {
                this.UpdateStatus(classRef, scripts);
            }).Every(1000);
        }

        private void UpdateStatus(ClassRef classRef, Script[] scripts)
        {
            var status = classRef.GetStatus(scripts);

            switch (status)
            {
                case ClassRefStatus.Full:
                    this.Status.SetColor(Color.green);
                    this.Status.tooltip = $"Class is found by name and id {this.GetTooltip(classRef)}";
                    break;
                case ClassRefStatus.Id:
                    this.Status.SetColor(Color.cyan);
                    this.Status.tooltip = $"Class is only found by id! {this.GetTooltip(classRef)}";
                    break;
                case ClassRefStatus.Name:
                    this.Status.SetColor(Color.yellow);
                    this.Status.tooltip = $"Class is only found by name! {this.GetTooltip(classRef)}";
                    break;
                case ClassRefStatus.None:
                    this.Status.SetColor(Color.red);
                    this.Status.tooltip = $"Class is not found! {this.GetTooltip(classRef)}";
                    break;
                case ClassRefStatus.Empty:
                    this.Status.SetColor(Color.black);
                    this.Status.tooltip = $"There is no name or id! {this.GetTooltip(classRef)}";
                    break;
            }
        }

        private string GetTooltip(ClassRef classRef)
        {
            return $"\nname: {classRef.Name ?? "-"}\nid: {classRef.Id ?? "-"}\nhash: {classRef.GetHashCode()}";
        }
    }
}