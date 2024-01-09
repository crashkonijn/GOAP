using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes.RunStates;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.GenTest
{
    [GoapId("VeryUnique-97508604-5cf4-4f1e-8d39-dbb74eed201f")]
    public class VeryUniqueAction : ActionBase<VeryUniqueAction.Data, VeryUniqueAction.Props>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, Data data)
        {
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            return ActionRunState.Stop;
        }

        public override void Stop(IMonoAgent agent, Data data)
        {
        }

        public override void Complete(IMonoAgent agent, Data data)
        {
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }
        }
        
        [Serializable]
        public class Props : IActionProperties
        {
            public string words;
            public int number;
        }
    }
}