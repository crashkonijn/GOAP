using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Generators;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class SensorList<TSensorType> : ListElementBase<TSensorType, CapabilitySensorElement>
        where TSensorType : BehaviourSensor, new()
    {
        private readonly CapabilityConfigScriptable scriptable;
        private readonly GeneratorScriptable generator;

        public SensorList(CapabilityConfigScriptable scriptable, GeneratorScriptable generator, List<TSensorType> sensors) : base(sensors)
        {
            this.scriptable = scriptable;
            this.generator = generator;
            
            this.Rebuild();
        }

        protected override CapabilitySensorElement CreateListItem(TSensorType item)
        {
            return new CapabilitySensorElement(this.scriptable, this.generator, item);
        }

        protected override void BindListItem(CapabilitySensorElement element, TSensorType item, int index)
        {
            element.Foldout.text = item.ToString();
            
            this.Bind(element, item);
        }

        private void Bind(CapabilitySensorElement element, TSensorType item)
        {
            switch (item)
            {
                case BehaviourMultiSensor multiSensor:
                    this.BindSensor(element, multiSensor);
                    break;
                case BehaviourTargetSensor targetSensor:
                    this.BindSensor(element, targetSensor);
                    break;
                case BehaviourWorldSensor worldSensor:
                    this.BindSensor(element, worldSensor);
                    break;
            }
        }

        private void BindSensor(CapabilitySensorElement element, BehaviourWorldSensor item)
        {
            element.SensorField.Bind(this.scriptable, item.sensor, this.generator.GetWorldSensors(), classRef =>
            {
                element.Foldout.text = item.ToString();
            });
            
            element.KeyField.Bind(this.scriptable, item.worldKey, this.generator.GetWorldKeys(), classRef =>
            {
            });
        }

        private void BindSensor(CapabilitySensorElement element, BehaviourTargetSensor item)
        {
            element.SensorField.Bind(this.scriptable, item.sensor, this.generator.GetTargetSensors(), classRef =>
            {
                element.Foldout.text = item.ToString();
            });
            
            element.KeyField.Bind(this.scriptable, item.targetKey, this.generator.GetTargetKeys(), classRef =>
            {
            });
        }

        private void BindSensor(CapabilitySensorElement element, BehaviourMultiSensor item)
        {
            element.SensorField.Bind(this.scriptable, item.sensor, this.generator.GetMultiSensors(), classRef =>
            {
                element.Foldout.text = item.ToString();
            });
        }
    }
}