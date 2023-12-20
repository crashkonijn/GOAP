using System;
using System.IO;

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

        public GenerationResult CreateTargetKey(string basePath, string name, string namespaceName)
        {
            if (name == String.Empty)
                return null;
            
            var template = this.LoadTemplate("target-key");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/TargetKeys/{name}.cs";
            this.StoreAtPath(result, path);
            
            return new GenerationResult
            {
                path = path,
                name = name,
                id = id
            };
        }

        public GenerationResult CreateWorldKey(string basePath, string name, string namespaceName)
        {
            if (name == String.Empty)
                return null;

            var template = this.LoadTemplate("world-key");
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/WorldKeys/{name}.cs";
            this.StoreAtPath(result, path);
            
            return new GenerationResult
            {
                path = path,
                name = name,
                id = id
            };
        }

        public GenerationResult CreateGoal(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("goal");
            name = name.Replace("Goal", "");

            if (name == String.Empty)
                return null;
            
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/Goals/{name}Goal.cs";
            this.StoreAtPath(result, path);
            
            return new GenerationResult
            {
                path = path,
                name = name,
                id = id
            };
        }
        
        public GenerationResult CreateAction(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("action");
            name = name.Replace("Action", "");

            if (name == String.Empty)
                return null;
            
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/Actions/{name}Action.cs";
            this.StoreAtPath(result, path);
            
            return new GenerationResult
            {
                path = path,
                name = name,
                id = id
            };
        }
        
        public GenerationResult CreateMultiSensor(string basePath, string name, string namespaceName)
        {
            if (name == String.Empty)
                return null;

            var template = this.LoadTemplate("multi-sensor");
            name = name.Replace("Sensor", "");
            
            var id = this.GetId(name);
            
            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/Sensors/Multi/{name}Sensor.cs";
            this.StoreAtPath(result, path);
            
            return new GenerationResult
            {
                path = path,
                name = name,
                id = id
            };
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

    public class GenerationResult
    {
        public string path;
        public string name;
        public string id;
    }
}