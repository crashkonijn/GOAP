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
        private class Action : GoapActionBase<Action.Data>
        {
            private bool isValid = true;
            
            public Action(bool isValid)
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

        [TestCase(false)]
        [TestCase(true)]
        public void Test(bool shouldBe)
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
            
            var actionData = new Action.Data();
            var action = new Action(shouldBe);
            action.SetConfig(config);

            var result = action.IsValid(actionReceiver, actionData);

            Assert.AreEqual(shouldBe, result);
        }
    }
}