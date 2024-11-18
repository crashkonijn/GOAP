using System;
using System.IO;
using CrashKonijn.Goap.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    [CustomEditor(typeof(GeneratorScriptable))]
    public class GeneratorEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var scriptable = (GeneratorScriptable) this.target;
            
            var root = new VisualElement();

            root.Add(new PropertyField(this.serializedObject.FindProperty("nameSpace")));
            
            var goals = this.CreateTextField(root, "Goals");
            var actions = this.CreateTextField(root, "Actions");
            var worldKeys = this.CreateTextField(root, "WorldKeys");
            var targetKeys = this.CreateTextField(root, "TargetKeys");
            
            // Create the Button
            var button = new Button(() =>
            {
                foreach (var name in goals.text.Split(new string[] { "\r\n" , "\n"}, StringSplitOptions.None))
                {
                    scriptable.CreateGoal(name);
                }
                foreach (var name in actions.text.Split(new string[] { "\r\n" , "\n"}, StringSplitOptions.None))
                {
                    scriptable.CreateAction(name);
                }
                foreach (var name in worldKeys.text.Split(new string[] { "\r\n" , "\n"}, StringSplitOptions.None))
                {
                    scriptable.CreateWorldKey(name);
                }
                foreach (var name in targetKeys.text.Split(new string[] { "\r\n" , "\n"}, StringSplitOptions.None))
                {
                    scriptable.CreateTargetKey(name);
                }
            });
            button.Add(new Label($"Generate"));
            root.Add(button);

            var check = new Button(() =>
            {
                Debug.Log(scriptable.scripts.actions.Length);
                
                var classes = ClassScanner.GetClasses(scriptable.nameSpace, Path.GetDirectoryName(AssetDatabase.GetAssetPath(scriptable)));

                Debug.Log("---Goals---");
                foreach (var script in classes.goals)
                {
                    Debug.Log($"{script.Type.Name}\n{script.Path}\n{script.Id}");
                }

                Debug.Log("---Actions---");
                foreach (var script in classes.actions)
                {
                    Debug.Log($"{script.Type.Name}\n{script.Path}\n{script.Id}");
                }

                Debug.Log("---WorldKeys---");
                foreach (var script in classes.worldKeys)
                {
                    Debug.Log($"{script.Type.Name}\n{script.Path}\n{script.Id}");
                }

                Debug.Log("---WorldSensors---");
                foreach (var script in classes.worldSensors)
                {
                    Debug.Log($"{script.Type.Name}\n{script.Path}\n{script.Id}");
                }

                Debug.Log("---TargetKeys---");
                foreach (var script in classes.targetKeys)
                {
                    Debug.Log($"{script.Type.Name}\n{script.Path}\n{script.Id}");
                }

                Debug.Log("---TargetSensors---");
                foreach (var script in classes.targetSensors)
                {
                    Debug.Log($"{script.Type.Name}\n{script.Path}\n{script.Id}");
                }
            });
            check.Add(new Label("Check"));
            root.Add(check);

            return root;
        }
        
        private TextField CreateTextField(VisualElement root, string label)
        {
            var textField = new TextField();
            textField.multiline = true;
            
            var labelElement = new Label(label);
            root.Add(labelElement);
            
            root.Add(textField);
            
            return textField;
        }
    }
}