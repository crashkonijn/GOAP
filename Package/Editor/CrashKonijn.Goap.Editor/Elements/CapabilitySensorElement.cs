using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Support.Generators;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.Elements
{
    public class CapabilitySensorElement : VisualElement, IFoldable
    {
        public Foldout Foldout { get; set; }
        public ClassRefField SensorField { get; set; }
        public ClassRefField KeyField { get; set; }
        
        public CapabilitySensorElement(CapabilityConfigScriptable scriptable, GeneratorScriptable generator, BehaviourSensor sensor)
        {
            this.Foldout = new Foldout
            {
                value = false,
            };
            this.Add(this.Foldout);

            this.SensorField = new ClassRefField();
            this.Foldout.Add(this.SensorField);

            if (sensor is BehaviourMultiSensor)
                return;
            
            this.KeyField = new ClassRefField();
            this.Foldout.Add(this.KeyField);
        }
    }
}