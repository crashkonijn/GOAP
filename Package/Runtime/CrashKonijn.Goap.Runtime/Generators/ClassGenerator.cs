using System;
using System.Collections.Generic;
using System.IO;

namespace CrashKonijn.Goap.Runtime
{
    public class ClassGenerator
    {
        private Dictionary<string, Script> scripts = new();

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private bool EnsureVariables(string basePath, string name, string namespaceName)
        {
            if (basePath == string.Empty)
                throw new GoapException("Base path cannot be empty!");

            if (name == string.Empty)
                return false;

            if (namespaceName == string.Empty)
                throw new GoapException("Namespace cannot be empty!");

            return true;
        }

        public Script CreateTargetKey(string basePath, string name, string namespaceName)
        {
            if (!this.EnsureVariables(basePath, name, namespaceName))
                return null;

            var template = this.LoadTemplate("target-key");
            var id = this.GetId(name);

            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/TargetKeys/{name}.cs";

            var created = this.StoreAtPath(result, path);

            return this.GetScript(id, path, name, !created);
        }

        public Script CreateWorldKey(string basePath, string name, string namespaceName)
        {
            if (!this.EnsureVariables(basePath, name, namespaceName))
                return null;

            var template = this.LoadTemplate("world-key");
            var id = this.GetId(name);

            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/WorldKeys/{name}.cs";
            var created = this.StoreAtPath(result, path);

            return this.GetScript(id, path, name, !created);
        }

        public Script CreateGoal(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("goal");
            name = name.Replace("Goal", "");

            if (!this.EnsureVariables(basePath, name, namespaceName))
                return null;

            var id = this.GetId(name);

            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/Goals/{name}Goal.cs";
            var created = this.StoreAtPath(result, path);

            return this.GetScript(id, path, name + "Goal", !created);
        }

        public Script CreateAction(string basePath, string name, string namespaceName)
        {
            var template = this.LoadTemplate("action");
            name = name.Replace("Action", "");

            if (!this.EnsureVariables(basePath, name, namespaceName))
                return null;

            var id = this.GetId(name);

            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/Actions/{name}Action.cs";
            var created = this.StoreAtPath(result, path);

            return this.GetScript(id, path, name + "Action", !created);
        }

        public Script CreateMultiSensor(string basePath, string name, string namespaceName)
        {
            if (!this.EnsureVariables(basePath, name, namespaceName))
                return null;

            var template = this.LoadTemplate("multi-sensor");
            name = name.Replace("Sensor", "");

            var id = this.GetId(name);

            var result = this.Replace(template, id, name, namespaceName);
            var path = $"{basePath}/Sensors/Multi/{name}Sensor.cs";
            var created = this.StoreAtPath(result, path);

            return this.GetScript(id, path, name + "Sensor", !created);
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

        private bool StoreAtPath(string content, string path)
        {
            this.EnsureDirectoryExists(Path.GetDirectoryName(path));

            if (File.Exists(path))
                return false;

            File.WriteAllText(path, content);
            return true;
        }

        private string LoadTemplate(string name)
        {
            var path = this.GetTemplatePath(name);

            return File.ReadAllText(path);
        }

        private string GetTemplatePath(string name)
        {
            var basePath = "Packages/com.crashkonijn.goap/Runtime/CrashKonijn.Goap.Runtime/Generators";

            return basePath + "/Templates/" + name + ".template";
        }

        private Script GetScript(string id, string path, string name, bool existing)
        {
            if (existing && this.scripts.TryGetValue(path, out var s))
                return s;

            var script = new Script
            {
                Id = id,
                Path = path,
                Name = name,
            };

            this.scripts.Add(path, script);
            return script;
        }
    }
}
