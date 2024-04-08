using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CrashKonijn.Goap.Editor.NodeViewer
{
    public class ClassFinder
    {
        public static List<Scene> GetAllValidScenes()
        {
            List<Scene> scenes = new();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                
                if (scene.IsValid() == false)
                    continue;

                scenes.Add(scene);
            }

            return scenes;
        }

        public static TComponent FindComponentOfTypeInScene<TComponent>(Scene scene)
        {
            var roots = scene.GetRootGameObjects();

            for (var j = 0; j < roots.Length; j++)
            {
                var component = roots[j].GetComponentInChildren<TComponent>();

                if (component != null)
                    return component;
            }

            return default;
        }

        public static IReadOnlyList<TComponent> FindComponentsOfTypeInScene<TComponent>(Scene scene)
        {
            var roots = scene.GetRootGameObjects();
            List<TComponent> components = new();

            for (var j = 0; j < roots.Length; j++)
            {
                components.AddRange(roots[j].GetComponentsInChildren<TComponent>());
            }

            return components;
        }

        public static TComponent FindComponentOfType<TComponent>()
        {
            var scenes = GetAllValidScenes();

            for (var i = 0; i < scenes.Count; i++)
            {
                var component = FindComponentOfTypeInScene<TComponent>(scenes[i]);

                if (component != null)
                    return component;
            }

            return default;
        }

        public static IReadOnlyList<TComponent> FindComponentsOfType<TComponent>()
        {
            var scenes = GetAllValidScenes();
            List<TComponent> components = new();

            for (var i = 0; i < scenes.Count; i++)
            {
                components.AddRange(FindComponentsOfTypeInScene<TComponent>(scenes[i]));
            }

            return components;
        }
    }
}