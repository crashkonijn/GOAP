using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class ScriptReferenceValidator
    {
        public IClassReferenceIssue[] CheckAll(CapabilityConfigScriptable capabilityConfig)
        {
            var generator = capabilityConfig.GetGenerator();
            var classes = generator.GetClasses();

            var issues = new List<IClassReferenceIssue>();

            capabilityConfig.goals.ForEach(goal =>
            {
                issues.Add(this.CheckReference(goal.goal, classes.goals, ClassRefType.Goal));

                goal.conditions.ForEach(condition =>
                {
                    issues.Add(this.CheckReference(condition.worldKey, classes.worldKeys, ClassRefType.WorldKey));
                });
            });

            capabilityConfig.actions.ForEach(action =>
            {
                issues.Add(this.CheckReference(action.action, classes.actions, ClassRefType.Action));
                issues.Add(this.CheckReference(action.target, classes.targetKeys, ClassRefType.TargetKey));

                action.conditions.ForEach(condition =>
                {
                    issues.Add(this.CheckReference(condition.worldKey, classes.worldKeys, ClassRefType.WorldKey));
                });

                action.effects.ForEach(effect =>
                {
                    issues.Add(this.CheckReference(effect.worldKey, classes.worldKeys, ClassRefType.WorldKey));
                });
            });

            capabilityConfig.worldSensors.ForEach(sensor =>
            {
                issues.Add(this.CheckReference(sensor.sensor, classes.worldSensors, ClassRefType.WorldSensor));
                issues.Add(this.CheckReference(sensor.worldKey, classes.worldKeys, ClassRefType.WorldKey));
            });

            capabilityConfig.targetSensors.ForEach(sensor =>
            {
                issues.Add(this.CheckReference(sensor.sensor, classes.targetSensors, ClassRefType.TargetSensor));
                issues.Add(this.CheckReference(sensor.targetKey, classes.targetKeys, ClassRefType.TargetKey));
            });

            capabilityConfig.multiSensors.ForEach(sensor =>
            {
                issues.Add(this.CheckReference(sensor.sensor, classes.multiSensors, ClassRefType.MultiSensor));
            });

            return issues.Where(x => x != null).ToArray();
        }

        private IClassReferenceIssue CheckReference(ClassRef reference, Script[] scripts, ClassRefType type)
        {
            var (status, match) = reference.GetMatch(scripts);

            switch (status)
            {
                case ClassRefStatus.Empty:
                    return new EmptyClassReferenceIssue(reference, type);
                case ClassRefStatus.None:
                    return new MissingClassReferenceIssue(reference, type);
                case ClassRefStatus.Name:
                    return new NameClassReferenceIssue(reference, match);
                case ClassRefStatus.Id:
                    return new IdClassReferenceIssue(reference, match);
            }

            return null;
        }
    }

    public interface IClassReferenceIssue
    {
        void Fix(GeneratorScriptable generator);
        string GetMessage();
    }

    public class EmptyClassReferenceIssue : IClassReferenceIssue
    {
        private readonly ClassRef reference;
        private readonly ClassRefType type;

        public EmptyClassReferenceIssue(ClassRef reference, ClassRefType type)
        {
            this.reference = reference;
            this.type = type;
        }

        public void Fix(GeneratorScriptable generator)
        {
            Debug.Log($"Unable to fix reference without name and id: {this.type}");
        }

        public string GetMessage()
        {
            return $"Reference without name and id: {this.type}";
        }
    }

    public class MissingClassReferenceIssue : IClassReferenceIssue
    {
        private readonly ClassRef reference;
        private readonly ClassRefType type;

        public MissingClassReferenceIssue(ClassRef reference, ClassRefType type)
        {
            this.reference = reference;
            this.type = type;
        }

        public void Fix(GeneratorScriptable generator)
        {
            var result = this.Generate(generator);

            if (result == null)
                return;

            this.reference.Id = result.Id;
            this.reference.Name = result.Name;

            Debug.Log($"Generated {result.Path}");
        }

        private Script Generate(GeneratorScriptable generator)
        {
#if UNITY_EDITOR
            switch (this.type)
            {
                case ClassRefType.Action:
                    return generator.CreateAction(this.reference.Name);
                case ClassRefType.Goal:
                    return generator.CreateGoal(this.reference.Name);
                case ClassRefType.TargetKey:
                    return generator.CreateTargetKey(this.reference.Name);
                case ClassRefType.WorldKey:
                    return generator.CreateWorldKey(this.reference.Name);
                case ClassRefType.TargetSensor:
                    Debug.Log("TargetSensors can't be generated. Please create them manually.");
                    break;
                case ClassRefType.WorldSensor:
                    Debug.Log("WorldSensors can't be generated. Please create them manually.");
                    break;
                case ClassRefType.MultiSensor:
                    return generator.CreateMultiSensor(this.reference.Name);
            }
#endif
            return null;
        }

        public string GetMessage()
        {
            return $"Class does not exist: {this.reference.Name} ({this.type})";
        }
    }

    public class NameClassReferenceIssue : IClassReferenceIssue
    {
        private readonly ClassRef reference;
        private readonly Script script;

        public NameClassReferenceIssue(ClassRef reference, Script script)
        {
            this.reference = reference;
            this.script = script;
        }

        public void Fix(GeneratorScriptable generator)
        {
            this.reference.Id = this.script.Id;
            Debug.Log($"Fixed {this.GetMessage()}");
        }

        public string GetMessage()
        {
            return $"Reference matched by name, but not by id: {this.reference.Name} ({this.script.Type.Name})";
        }
    }

    public class IdClassReferenceIssue : IClassReferenceIssue
    {
        private readonly ClassRef reference;
        private readonly Script script;

        public IdClassReferenceIssue(ClassRef reference, Script script)
        {
            this.reference = reference;
            this.script = script;
        }

        public void Fix(GeneratorScriptable generator)
        {
            this.reference.Name = this.script.Type.Name;
            Debug.Log($"Fixed {this.GetMessage()}");
        }

        public string GetMessage()
        {
            return $"Reference matched by id, but not by name: {this.reference.Id} ({this.script.Type.Name})";
        }
    }

    public enum ClassRefType
    {
        Goal,
        Action,
        TargetKey,
        WorldKey,
        TargetSensor,
        WorldSensor,
        MultiSensor,
    }
}
