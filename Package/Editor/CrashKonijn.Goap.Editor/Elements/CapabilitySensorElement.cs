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

        public Label LabelField { get; set; }
        
        public CapabilitySensorElement(CapabilityConfigScriptable scriptable, GeneratorScriptable generator, BehaviourSensor sensor)
        {
            this.Foldout = new Foldout
            {
                value = false,
            };
            this.Add(this.Foldout);
            
            this.Foldout.Add(new Card((card) =>
            {
                var sensorLabel = new LabeledField<ClassRefField>("Sensor");
                this.SensorField = sensorLabel.Field;
                card.Add(sensorLabel);

                if (sensor is BehaviourMultiSensor)
                {
                    var sensorsLabel = new LabeledField<Label>("Keys");
                    this.LabelField = sensorsLabel.Field;
                    card.Add(sensorsLabel);
                    return;
                }
            
                var keyLabel = new LabeledField<ClassRefField>("Key");
                this.KeyField = keyLabel.Field;
                card.Add(keyLabel);
            }));
        }
    }
}