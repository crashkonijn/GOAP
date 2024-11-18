using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Docs.GettingStarted.Behaviours;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Docs.GettingStarted.Actions
{
    [GoapId("PickupPear-06ef21a4-059b-4314-800a-e7c2622637fb")]
    public class PickupPearAction : GoapActionBase<PickupPearAction.Data>
    {
        // This method is called every frame while the action is running
        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            // Instead of using a timer, we can use the Wait ActionRunState.
            // The system will wait for the specified time before completing the action
            // Whilst waiting, the Perform method won't be called again
            return ActionRunState.WaitThenComplete(0.5f);
        }

        // This method is called when the action is completed
        public override void Complete(IMonoAgent agent, Data data)
        {
            if (data.Target is not TransformTarget transformTarget)
                return;
            
            data.DataBehaviour.pearCount++;
            GameObject.Destroy(transformTarget.Transform.gameObject);
        }

        // The action class itself must be stateless!
        // All data should be stored in the data class
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            
            // When using the GetComponent attribute, the system will automatically inject the reference
            [GetComponent]
            public DataBehaviour DataBehaviour { get; set; }
        }
    }
}