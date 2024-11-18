using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public class ProactiveControllerBehaviour : MonoBehaviour, IGoapController
    {
        private ProactiveController controller = new();

        [Tooltip("Only updates during Awake")]
        [SerializeField]
        private float resolveTime = 1f;

        public void Awake()
        {
            this.controller.ResolveTime = this.resolveTime;
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
