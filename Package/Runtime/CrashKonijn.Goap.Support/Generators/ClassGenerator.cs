using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CrashKonijn.Goap.Generators
{
    public class ClassGenerator
    {
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void CreateTargetKey(string basePath, string name, string namespaceName)
        {
            if (name == String.Empty)
                return;
            
            var template = this.LoadTemplate("target-key");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/TargetKeys/{name}.cs");
        }

        public void CreateWorldKey(string basePath, string name, string namespaceName)
        {
            if (name == String.Empty)
                return;

            var template = this.LoadTemplate("world-key");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/WorldKeys/{name}.cs");
        }

        public void CreateGoal(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("goal");
            name = name.Replace("Goal", "");

            if (name == String.Empty)
                return;
            
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/Goals/{name}Goal.cs");
        }
        
        public void CreateAction(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("action");
            name = name.Replace("Action", "");

            if (name == String.Empty)
                return;
            
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/Actions/{name}Action.cs");
        }

        private string GetId(string name)
        {
            return $"{name}-{Guid.NewGuid().ToString()}";
        }

        private string Replace(string template, string id, string name, string namespaceName)
        {
            template = template.Replace("{{id}}", id);
            template = template.Replace("{{name}}", name);
            template = template.Replace("{{namespace}}", namespaceName);

            return template;
        }

        private void StoreAtPath(string content, string path)
        {
            this.EnsureDirectoryExists(Path.GetDirectoryName(path));

            if (File.Exists(path))
                return;
            
            File.WriteAllText(path, content);
        }

        private string LoadTemplate(string name)
        {
            var path = this.GetTemplatePath(name);
            
            return File.ReadAllText(path);
        }
        
        private string GetTemplatePath(string name)
        {
            var basePath = "Packages/com.crashkonijn.goap/Runtime/CrashKonijn.Goap.Support/Generators";
            
            return basePath + "/Templates/" + name + ".template";
        }
    }
}