using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class ProactiveControllerBehaviour : MonoBehaviour, IGoapController
    {
        private ProactiveController controller = new();
        
        public float ResolveTime {
            get => this.controller.ResolveTime;
            set => this.controller.ResolveTime = value;
        }
        
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