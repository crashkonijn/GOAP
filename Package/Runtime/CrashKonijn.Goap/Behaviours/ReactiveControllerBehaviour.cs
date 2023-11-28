using CrashKonijn.Goap.Classes.Controllers;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class ReactiveControllerBehaviour : MonoBehaviour, IGoapController
    {
        private ReactiveController controller = new();
        
        public void Initialize(IGoap goap)
        {
            this.controller.Initialize(goap);
        }
        
        private void OnDisable()
        {
            this.controller.Disable();
        }
        
        public void OnUpdate()
        {
            this.controller.OnUpdate();
        }
        
        public void OnLateUpdate()
        {
            this.controller.OnLateUpdate();
        }
    }
}