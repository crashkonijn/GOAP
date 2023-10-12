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
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public void CreateTargetKey(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("target-key");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/TargetKeys/{name}.cs");
        }

        public void CreateWorldKey(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("world-key");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/WorldKeys/{name}.cs");
        }

        public void CreateGoal(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("goal");
            name = name.Replace("Goal", "");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            this.StoreAtPath(result, $"{basePath}/Goals/{name}Goal.cs");
        }
        
        public void CreateAction(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("action");
            name = name.Replace("Action", "");
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
            
            System.IO.File.WriteAllText(path, content);
        }

        private string LoadTemplate(string name)
        {
            var path = this.GetTemplatePath(name);
            
            return System.IO.File.ReadAllText(path);
        }
        
        private string GetTemplatePath(string name)
        {
            var basePath = "Packages/com.crashkonijn.goap/Runtime/CrashKonijn.Goap.Support/Generators";
            
            return basePath + "/Templates/" + name + ".template";
        }
    }
}