using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using UnityEditor;

namespace CrashKonijn.Goap.Editor
{
    public class SensorList<TSensorType> : ListElementBase<TSensorType, CapabilitySensorElement>
        where TSensorType : CapabilitySensor, new()
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public SensorList(SerializedObject serializedObject, CapabilityConfigScriptable scriptable, GeneratorScriptable generator, List<TSensorType> list, string property) : base(serializedObject.FindProperty(property), list)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilitySensorElement CreateListItem(SerializedProperty property, TSensorType item)
        {
            return new CapabilitySensorElement(this.scriptable, this.generator, item);
        }

        protected override void BindListItem(SerializedProperty property, CapabilitySensorElement element, TSensorType item, int index)
        {
            element.Foldout.text = this.GetName(item);
            
            this.Bind(element, item);
        }

        private void Bind(CapabilitySensorElement element, TSensorType item)
        {
            switch (item)
            {
                case CapabilityMultiSensor multiSensor:
                    this.BindSensor(element, multiSensor);
                    break;
                case CapabilityTargetSensor targetSensor:
                    this.BindSensor(element, targetSensor);
                    break;
                case CapabilityWorldSensor worldSensor:
                    this.BindSensor(element, worldSensor);
                    break;
            }
        }

        private void BindSensor(CapabilitySensorElement element, CapabilityWorldSensor item)
        {
            element.SensorField.Bind(this.scriptable, item.sensor, this.generator.GetWorldSensors(), classRef =>
            {
                element.Foldout.text = this.GetName(item);
            });
            
            element.KeyField.Bind(this.scriptable, item.worldKey, this.generator.GetWorldKeys(), classRef =>
            {
                element.Foldout.text = this.GetName(item);
            });
        }

        private void BindSensor(CapabilitySensorElement element, CapabilityTargetSensor item)
        {
            element.SensorField.Bind(this.scriptable, item.sensor, this.generator.GetTargetSensors(), classRef =>
            {
                element.Foldout.text = this.GetName(item);
            });
            
            element.KeyField.Bind(this.scriptable, item.targetKey, this.generator.GetTargetKeys(), classRef =>
            {
                element.Foldout.text = this.GetName(item);
            });
        }

        private void BindSensor(CapabilitySensorElement element, CapabilityMultiSensor item)
        {
            element.SensorField.Bind(this.scriptable, item.sensor, this.generator.GetMultiSensors(), classRef =>
            {
                element.Foldout.text = this.GetName(item);
            });
            
            var sensors  = this.GetMultiSensors(item.sensor, this.generator.GetMultiSensors());

            element.LabelField.text = "- " + string.Join("\n- ", sensors);
            
            element.Foldout.text = $"{item} ({sensors.Length})";
        }

        public string[] GetMultiSensors(ClassRef classRef, Script[] scripts)
        {
            var match = classRef.GetMatch(scripts);
            
            if (match.status == ClassRefStatus.None)
                return Array.Empty<string>();
            
            // Create instance of type
            var instance = (IMultiSensor) Activator.CreateInstance(match.script.Type);
            
            return instance.GetSensors();
        }

        public string GetName(CapabilitySensor item)
        {
            if (item is CapabilityMultiSensor multiSensor)
            {
                return multiSensor.ToString();
            }

            if (item is CapabilityWorldSensor worldSensor)
            {
                var scopes = new List<string>(){ worldSensor.worldKey.Name };
                scopes.AddRange(this.GetScopes(worldSensor.sensor, this.generator.GetWorldSensors()));

                return $"{worldSensor.sensor.Name} ({string.Join(", ", scopes)})";
            }

            if (item is CapabilityTargetSensor targetSensor)
            {
                var scopes = new List<string>(){ targetSensor.targetKey.Name };
                scopes.AddRange(this.GetScopes(targetSensor.sensor, this.generator.GetTargetSensors()));

                return $"{targetSensor.sensor.Name} ({string.Join(", ", scopes)})";
            }

            return "";
        }

        private string[] GetScopes(ClassRef classRef, Script[] scripts)
        {
            var (status, match) = classRef.GetMatch(scripts);

            if (status == ClassRefStatus.None)
                return Array.Empty<string>();
            
            if (status == ClassRefStatus.Empty)
                return Array.Empty<string>();
            
            var scopes = new List<string>();
            
            if (typeof(ILocalSensor).IsAssignableFrom(match.Type))
                scopes.Add("local");
            
            if (typeof(IGlobalSensor).IsAssignableFrom(match.Type))
                scopes.Add("global");

            return scopes.ToArray();
        }
    }
}