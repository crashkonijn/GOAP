using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    public class GraphBuilderTests
    {
        private class IsHungryKey : IWorldKey
        {
            public string Name => "IsHungry";
        }

        private class HasAppleKey : IWorldKey
        {
            public string Name => "HasApple";
        }

        [Test]
        public void Build_GivenActions_ReturnsGraph()
        {
            // Arrange
            var isHungryKey = new IsHungryKey();
            var hasAppleKey = new HasAppleKey();

            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new ICondition[]
            {
                new Condition
                {
                    WorldKey = isHungryKey,
                    Comparison = Comparison.SmallerThan,
                    Amount = 1,
                },
            });

            var eatAppleAction = Substitute.For<IGoapAction>();
            eatAppleAction.Conditions.Returns(new ICondition[]
            {
                new Condition
                {
                    WorldKey = hasAppleKey,
                    Comparison = Comparison.GreaterThan,
                    Amount = 0,
                },
            });
            eatAppleAction.Effects.Returns(new IEffect[]
            {
                new Effect
                {
                    WorldKey = isHungryKey,
                    Increase = false,
                },
            });

            var getAppleAction = Substitute.For<IGoapAction>();
            getAppleAction.Conditions.Returns(new ICondition[]
            {
                new Condition
                {
                    WorldKey = hasAppleKey,
                    Comparison = Comparison.SmallerThan,
                    Amount = 1,
                },
            });
            getAppleAction.Effects.Returns(new IEffect[]
            {
                new Effect
                {
                    WorldKey = hasAppleKey,
                    Increase = true,
                },
            });

            var actions = new List<IConnectable>
            {
                goal,
                eatAppleAction,
                getAppleAction,
            };

            var keyResolver = new KeyResolver();
            var graphBuilder = new GraphBuilder(keyResolver);

            // Act
            var graph = graphBuilder.Build(actions);

            // Assert
            Assert.IsNotNull(graph);
            Assert.IsNotNull(graph.RootNodes);
            Assert.AreEqual(1, graph.RootNodes.Count);
            Assert.IsNotNull(graph.ChildNodes);
            Assert.AreEqual(2, graph.ChildNodes.Count);
            Assert.IsNotNull(graph.UnconnectedNodes);
            Assert.AreEqual(0, graph.UnconnectedNodes.Length);

            var goalNode = graph.RootNodes[0];
            Assert.IsNotNull(goalNode);
            Assert.AreEqual(goal, goalNode.Action);

            var eatAppleNode = goalNode.Conditions[0].Connections[0];
            Assert.IsNotNull(eatAppleNode);
            Assert.AreEqual(eatAppleAction, eatAppleNode.Action);

            var getAppleNode = eatAppleNode.Conditions[0].Connections[0];
            Assert.IsNotNull(getAppleNode);
            Assert.AreEqual(getAppleAction, getAppleNode.Action);
        }

        [Test]
        public void Build_GivenActionWithUnmatchedEffects_ShouldBeUnconnected()
        {
            // Arrange
            var isHungryKey = new IsHungryKey();
            var hasAppleKey = new HasAppleKey();
            
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new ICondition[]
            {
                new Condition
                {
                    WorldKey = isHungryKey,
                    Comparison = Comparison.SmallerThan,
                    Amount = 1,
                },
            });
            
            var pickupAppleAction = Substitute.For<IGoapAction>();
            pickupAppleAction.Effects.Returns(new IEffect[]
            {
                new Effect
                {
                    WorldKey = hasAppleKey,
                    Increase = true,
                },
            });
            
            var actions = new List<IConnectable>
            {
                goal,
                pickupAppleAction
            };

            var keyResolver = new KeyResolver();
            var graphBuilder = new GraphBuilder(keyResolver);

            // Act
            var graph = graphBuilder.Build(actions);
            
            // Assert
            Assert.IsNotNull(graph);
            Assert.IsNotNull(graph.RootNodes);
            Assert.AreEqual(1, graph.RootNodes.Count);
            Assert.IsNotNull(graph.ChildNodes);
            Assert.AreEqual(0, graph.ChildNodes.Count);
            Assert.IsNotNull(graph.UnconnectedNodes);
            Assert.AreEqual(1, graph.UnconnectedNodes.Length);
            
            Assert.AreEqual(goal, graph.RootNodes[0].Action);
            Assert.AreEqual(pickupAppleAction, graph.UnconnectedNodes[0].Action);
        }

        [Test]
        public void Build_GivenOppositeConditions_ShouldNotBeConnected()
        {
            // Arrange
            var isHungryKey = new IsHungryKey();
            
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new ICondition[]
            {
                new Condition
                {
                    WorldKey = isHungryKey,
                    Comparison = Comparison.SmallerThan,
                    Amount = 1,
                },
            });
            
            var starveAction = Substitute.For<IGoapAction>();
            starveAction.Effects.Returns(new IEffect[]
            {
                new Effect
                {
                    WorldKey = isHungryKey,
                    Increase = true,
                },
            });
            
            var actions = new List<IConnectable>
            {
                goal,
                starveAction
            };
            
            var keyResolver = new KeyResolver();
            var graphBuilder = new GraphBuilder(keyResolver);
            
            // Act
            var graph = graphBuilder.Build(actions);
            
            // Assert
            Assert.IsNotNull(graph);
            Assert.IsNotNull(graph.RootNodes);
            Assert.AreEqual(1, graph.RootNodes.Count);
            Assert.IsNotNull(graph.ChildNodes);
            Assert.AreEqual(0, graph.ChildNodes.Count);
            Assert.IsNotNull(graph.UnconnectedNodes);
            Assert.AreEqual(1, graph.UnconnectedNodes.Length);
            
            Assert.AreEqual(goal, graph.RootNodes[0].Action);
            Assert.AreEqual(starveAction, graph.UnconnectedNodes[0].Action);
        }

        private class LowOnMoney : IWorldKey
        {
            public string Name => "LowOnMoney";
        }

        private class CrimeCommitted : IWorldKey
        {
            public string Name => "CrimeCommitted";
        }

        [Test]
        public void Build_GivenActionsWithConflictingConditions_ShouldNotConnect()
        {
            // Arrange
            var lowOnMoneyKey = new LowOnMoney();
            var crimeCommittedKey = new CrimeCommitted();

            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new ICondition[]
            {
                new Condition
                {
                    WorldKey = lowOnMoneyKey,
                    Comparison = Comparison.SmallerThan,
                    Amount = 1,
                },
                new Condition
                {
                    WorldKey = crimeCommittedKey,
                    Comparison = Comparison.SmallerThan,
                    Amount = 1,
                },
            });

            var robBankAction = Substitute.For<IGoapAction>();
            robBankAction.Effects.Returns(new IEffect[]
            {
                new Effect
                {
                    WorldKey = lowOnMoneyKey,
                    Increase = false,
                },
                new Effect
                {
                    WorldKey = crimeCommittedKey,
                    Increase = true,
                },
            });

            var pinMoneyAction = Substitute.For<IGoapAction>();
            pinMoneyAction.Effects.Returns(new IEffect[]
            {
                new Effect
                {
                    WorldKey = lowOnMoneyKey,
                    Increase = false,
                },
            });

            var actions = new List<IConnectable>
            {
                goal,
                robBankAction,
                pinMoneyAction,
            };

            var keyResolver = new KeyResolver();
            var graphBuilder = new GraphBuilder(keyResolver);

            // Act
            var graph = graphBuilder.Build(actions);


            // Assert
            Assert.IsNotNull(graph);
            Assert.IsNotNull(graph.RootNodes);
            Assert.AreEqual(1, graph.RootNodes.Count);
            Assert.IsNotNull(graph.ChildNodes);
            Assert.AreEqual(1, graph.ChildNodes.Count);
            Assert.IsNotNull(graph.UnconnectedNodes);
            Assert.AreEqual(1, graph.UnconnectedNodes.Length);

            Assert.AreEqual(goal, graph.RootNodes[0].Action);
            Assert.AreEqual(robBankAction, graph.UnconnectedNodes[0].Action);
            Assert.AreEqual(pinMoneyAction, graph.ChildNodes[0].Action);
        }
    }
}
