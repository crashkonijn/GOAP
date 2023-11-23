using System;
using System.Linq;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Loaders;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
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
            this.SelectField.formatListItemCallback = item => item.type.Name;
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
                this.NameField.SetValueWithoutNotify(evt.newValue.type.Name);
                classRef.Name = evt.newValue.type.Name;
                classRef.Id = evt.newValue.id;
                this.SelectField.SetValueWithoutNotify(null);
                onValueChanged(classRef);
                this.UpdateStatus(classRef, scripts);
                EditorUtility.SetDirty(scriptable); // Mark the scriptable object as dirty
            });
            
            this.UpdateStatus(classRef, scripts);
        }

        private void UpdateStatus(ClassRef classRef, Script[] scripts)
        {
            var status = classRef.GetStatus(scripts);

            switch (status)
            {
                case ClassRefStatus.Full:
                    this.Status.SetColor(Color.green);
                    break;
                case ClassRefStatus.Id:
                    this.Status.SetColor(Color.cyan);
                    break;
                case ClassRefStatus.Name:
                    this.Status.SetColor(Color.yellow);
                    break;
                case ClassRefStatus.None:
                    this.Status.SetColor(Color.red);
                    break;
            }
        }
    }
}