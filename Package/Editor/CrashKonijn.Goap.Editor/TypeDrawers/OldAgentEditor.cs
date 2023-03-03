// using CrashKonijn.Goap.Behaviours;
// using UnityEditor;
// using UnityEditor.IMGUI.Controls;
// using UnityEngine;
//
// namespace CrashKonijn.Goap.Editor.TypeDrawers
// {
//     // [CustomEditor(typeof(AgentBehaviour))]
//     public class OldAgentEditor : UnityEditor.Editor
//     {
//         private TreeViewState treeViewState;
//
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//
//             if (!Application.isPlaying)
//                 return;
//             
//             var agent = (AgentBehaviour) this.target;
//             
//             EditorGUILayout.LabelField("Goal", agent.CurrentGoal?.ToString());
//             EditorGUILayout.LabelField("Action", agent.CurrentAction?.ToString());
//
//             // this.treeViewState ??= new TreeViewState();
//             //
//             // var dataTree = new NestedDataRenderer(this.treeViewState, agent.CurrentActionData);
//             //
//             // dataTree.OnGUI(new Rect(0, 400, 300, 200));
//
//             
//             EditorGUILayout.LabelField("ActionData:", agent.CurrentActionData?.ToString());
//             GUILayout.BeginVertical( "box");
//             this.DrawProperties(agent.CurrentActionData);
//             GUILayout.EndVertical();
//
//             if (agent.WorldData == null)
//                 return;
//             
//             EditorGUILayout.LabelField("WorldData:");
//             GUILayout.BeginVertical( "box");
//
//             EditorGUI.indentLevel = 0;
//             EditorGUILayout.LabelField ("Target");
//             EditorGUI.indentLevel = 1;
//             
//             foreach (var (key, value) in agent.WorldData.Targets)
//             {
//                 EditorGUILayout.LabelField(key.Name, value.Position.ToString());
//             }
//
//             EditorGUI.indentLevel = 0;
//             EditorGUILayout.LabelField ("States");
//             EditorGUI.indentLevel = 1;
//             
//             foreach (var worldKey in agent.WorldData.States)
//             {
//                 EditorGUILayout.LabelField(worldKey.Name);
//             }
//             
//             GUILayout.EndVertical();
//         }
//
//         private void DrawData(AgentBehaviour agent)
//         {
//             
//         }
//
//         private void DrawProperties(object obj, int indentation = 0)
//         {
//             if (obj == null)
//                 return;
//
//             EditorGUI.indentLevel = indentation;
//             
//             var properties = obj.GetType().GetProperties();
//             
//             foreach (var property in properties)
//             {
//                 var value = property.GetValue(obj);
//                 
//                 if (value == null)
//                     continue;
//
//                 if (value.GetType().Namespace.StartsWith("Unity") || value.GetType().Namespace.StartsWith("System"))
//                 {
//                     EditorGUILayout.LabelField (property.Name, value?.ToString());
//                     continue;
//                 }
//
//                 // if (value.GetType().IsClass)
//                 // {
//                 //     EditorGUILayout.LabelField (property.Name);
//                 //     this.DrawProperties(value, indentation + 1);
//                 //     continue;
//                 // }
//
//                 EditorGUILayout.LabelField (property.Name, value?.ToString());
//             }
//         }
//     }
// }