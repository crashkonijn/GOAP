using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.TypeDrawers
{
    [CustomEditor(typeof(GoapSetFactoryBase), true)]
    public class GoapSetFactoryBaseEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var factory = (GoapSetFactoryBase) this.target;
            
            var root = new VisualElement();

            var validateButton = new Button(() =>
            {
                var validator = new GoapSetConfigValidatorRunner();
                var results = validator.Validate(factory.Create());
                
                foreach (var error in results.GetErrors())
                {
                    Debug.LogError(error);
                }
            
                foreach (var warning in results.GetWarnings())
                {
                    Debug.LogWarning(warning);
                }
                
                if (!results.HasErrors() && !results.HasWarnings())
                    Debug.Log("No errors or warnings found!");
            });
            
            validateButton.Add(new Label("Validate"));

            root.Add(validateButton);
            
            return root;
        }
    }
}