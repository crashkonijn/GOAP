// using System;
// using System.Collections.Generic;
// using System.Linq;
// using LamosInteractive.Goap.Unity.Classes;
// using LamosInteractive.Goap.Unity.Interfaces;
// using LamosInteractive.Goap.Unity.Scriptables;
// using UnityEditor;
//
// namespace LamosInteractive.Goap.Unity.Editor
// {
//     [CustomEditor(typeof(ActionConfig))]
//     public class ActionConfigEditor : UnityEditor.Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             var config = (ActionConfig) this.target;
//             
//             base.OnInspectorGUI();
//
//             config.actionClass = this.DrawTypeSelect(config.actionClass, this.GetAllOfType<IActionBase>());
//         }
//
//         private string DrawTypeSelect(string currentValue, List<string> options)
//         {
//             var index = options.FindIndex(x => x == currentValue);
//
//             index = EditorGUILayout.Popup("ActionClass", index, options.ToArray());
//
//             if (index == -1)
//                 return "";
//
//             return options[index];
//         }
//
//         private List<string> GetAllOfType<TType>()
//         {
//             var type = typeof(TType);
//             
//             return AppDomain.CurrentDomain.GetAssemblies()
//                     .Where(a => !a.IsDynamic)
//                     .SelectMany(a => a.GetTypes())
//                     .Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
//                     .Select(x => x.AssemblyQualifiedName).ToList();
//         }
//     }
// }