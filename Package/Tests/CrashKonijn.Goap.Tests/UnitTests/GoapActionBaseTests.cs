using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CrashKonijn.Goap.UnitTests
{
    public class GoapActionBaseTests
    {
        private class ValidAction : GoapActionBase<ValidAction.Data>
        {
            private bool isValid = true;
            
            public ValidAction(bool isValid)
            {
                this.isValid = isValid;
            }

            public override bool IsValid(IActionReceiver agent, Data data)
            {
                return this.isValid;
            }

            public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
            {
                throw new System.NotImplementedException();
            }
            
            public class Data : IActionData
            {
                public ITarget Target { get; set; }
            }
        }
        
        private class CostAction : GoapActionBase<CostAction.Data>
        {
            private float cost;
            
            public CostAction(float cost)
            {
                this.cost = cost;
            }

            public override float GetCost(IActionReceiver agent, IComponentReference references, ITarget target)
            {
                return this.cost;
            }

            public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
            {
                throw new System.NotImplementedException();
            }
            
            public class Data : IActionData
            {
                public ITarget Target { get; set; }
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IsValid_ShouldCall_CustomIsValid(bool shouldBe)
        {
            var gameObject = new GameObject();
            var goapActionProvider = gameObject.AddComponent<GoapActionProvider>();
            
            var injector = Substitute.For<IDataReferenceInjector>();
            injector.GetCachedComponent<GoapActionProvider>().Returns(goapActionProvider);
            
            var actionReceiver = Substitute.For<IActionReceiver>();
            actionReceiver.Injector.Returns(injector);
            
            var config = Substitute.For<IActionConfig>();
            config.RequiresTarget.Returns(false);
            config.ValidateConditions.Returns(false);
            
            var actionData = new ValidAction.Data();
            var action = new ValidAction(shouldBe);
            action.SetConfig(config);

            var result = action.IsValid(actionReceiver, actionData);

            Assert.AreEqual(shouldBe, result);
        }

        [TestCase(20f)]
        [TestCase(10f)]
        public void IsValid_ShouldCall_CustomIsValid(float cost)
        {
            var gameObject = new GameObject();
            var goapActionProvider = gameObject.AddComponent<GoapActionProvider>();
            
            var injector = Substitute.For<IDataReferenceInjector>();
            injector.GetCachedComponent<GoapActionProvider>().Returns(goapActionProvider);
            
            var actionReceiver = Substitute.For<IActionReceiver>();
            actionReceiver.Injector.Returns(injector);
            
            var config = Substitute.For<IActionConfig>();
            config.RequiresTarget.Returns(false);
            config.ValidateConditions.Returns(false);
            
            var action = new CostAction(cost);
            action.SetConfig(config);

            var result = action.GetCost(actionReceiver, injector, null);

            Assert.AreEqual(cost, result);
        }
    }
}