using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class ManualControllerBehaviour : MonoBehaviour, IGoapController
    {
        private ManualController controller = new();

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
