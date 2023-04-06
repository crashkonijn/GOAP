﻿using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;
using Demos.Shared.Behaviours;
using UnityEngine;

namespace Demos.Complex.Actions
{
    public class EatAction : ActionBase<EatAction.Data>
    {

        public override void OnStart(IMonoAgent agent, Data data)
        {
            data.Eatable = data.Inventory.Get<IEatable>().FirstOrDefault();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.Eatable == null)
                return ActionRunState.Stop;
            
            var eatNutrition = context.DeltaTime * 20f;
            data.Eatable.NutritionValue -= eatNutrition;
            data.Hunger.hunger -= eatNutrition;

            if (data.Eatable.NutritionValue > 0)
                return ActionRunState.Continue;

            data.Inventory.Remove(data.Eatable);
            GameObject.Destroy(data.Eatable.gameObject);
            
            return ActionRunState.Stop;
        }

        public override void OnEnd(IMonoAgent agent, Data data)
        {
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public IEatable Eatable { get; set; }
            
            [GetComponent]
            public ComplexInventoryBehaviour Inventory { get; set; }
            
            [GetComponent]
            public HungerBehaviour Hunger { get; set; }
        }
    }
}