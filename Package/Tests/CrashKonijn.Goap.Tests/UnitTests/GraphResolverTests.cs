using System;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
using CrashKonijn.Goap.UnitTests.Data;
using CrashKonijn.Goap.UnitTests.Interfaces;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using ITestAction = CrashKonijn.Goap.UnitTests.Interfaces.ITestAction;

namespace CrashKonijn.Goap.UnitTests
{
    public class GraphResolverTests
    {
        private class TestAction : TestNode<ITestAction>
        {
            public TestAction(string name) : base(name)
            {
            }

            public new ITestAction ToMock()
            {
                var mock = base.ToMock();

                mock.Name.Returns(this.Name);

                return mock;
            }
        }
        
        private class TestGoal : TestNode<ITestGoal>
        {
            public TestGoal(string name) : base(name)
            {
            }

            public new ITestGoal ToMock()
            {
                var mock = base.ToMock();

                mock.Name.Returns(this.Name);

                return mock;
            }
        }
        
        private class TestNode<T>
            where T : class, IConnectable
        {
            public string Name { get; }
            public Guid Guid { get; } = Guid.NewGuid();
            public IEffect[] Effects { get; set; } = {};
            public ICondition[] Conditions { get; set; } = {};

            public TestNode(string name)
            {
                this.Name = name;
            }

            public T ToMock()
            {
                var action = Substitute.For<T>();
                action.Conditions.Returns(this.Conditions);
                action.Effects.Returns(this.Effects);
                action.Guid.Returns(this.Guid);

                return action;
            }
        }
        
        [Test]
        public void Resolve_WithNoActions_ReturnsEmptyList()
        {
            // Arrange
            var actions = Array.Empty<IConnectable>();
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(new []{ false }, Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(new [] { false }, Allocator.TempJob),
                Positions = new NativeArray<float3>(new [] { float3.zero }, Allocator.TempJob),
                Costs = new NativeArray<float>(new []{ 1f }, Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(new [] { false }, Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().BeEmpty();
        }
        
        [Test]
        public void Resolve_WithOneExecutableConnection_ReturnsAction()
        {
            // Arrange
            var connection = new TestConnection("connection");
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { connection }
            }.ToMock();
            var action = new TestAction("action")
            {
                Effects = new IEffect[] { connection }
            }.ToMock();
            
            var actions = new IConnectable[] { goal, action };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(new []{ true, true }, Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(new [] { false, true }, Allocator.TempJob),
                Positions = new NativeArray<float3>(new []{ float3.zero, float3.zero }, Allocator.TempJob),
                Costs = new NativeArray<float>(new []{ 1f, 1f }, Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(new [] { false }, Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().Be(action);
        }
        
        [Test]
        public void Resolve_WithMultipleConnections_ReturnsExecutableAction()
        {
            // Arrange
            var connection = new TestConnection("connection");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { connection }
            }.ToMock();
            var firstAction = new TestAction("action1")
            {
                Effects = new IEffect[] { connection }
            }.ToMock();
            var secondAction = new TestAction("action2")
            {
                Effects = new IEffect[] { connection }
            }.ToMock();
            var thirdAction = new TestAction("action3")
            {
                Effects = new IEffect[] { connection }
            }.ToMock();
            
            var actions = new IConnectable[] { goal, firstAction, secondAction, thirdAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(new []{ true, true, true ,true }, Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(new [] { false, false, false, true }, Allocator.TempJob),
                Positions = new NativeArray<float3>(new []{ float3.zero, float3.zero, float3.zero, float3.zero }, Allocator.TempJob),
                Costs = new NativeArray<float>(new []{ 1f, 1f, 1f, 1f }, Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(new []{ false }, Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().Be(thirdAction);
        }
        
        [Test]
        public void Resolve_WithNestedConnections_ReturnsExecutableAction()
        {
            // Arrange
            var connection = new TestConnection("connection");
            var connection2 = new TestConnection("connection2");
            var connection3 = new TestConnection("connection3");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { connection }
            }.ToMock();
            var action1 = new TestAction("action1")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection2 }
            }.ToMock();
            var action2 = new TestAction("action2")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection3 }
            }.ToMock();
            var action11 = new TestAction("action11")
            {
                Effects = new IEffect[] { connection2 }
            }.ToMock();
            var action22 = new TestAction("action22")
            {
                Effects = new IEffect[] { connection3 }
            }.ToMock();
            
            var actions = new IConnectable[] { goal, action1, action2, action11, action22 };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            var executableBuilder = resolver.GetExecutableBuilder();
            var conditionBuilder = resolver.GetConditionBuilder();

            executableBuilder.SetExecutable(action11, true);
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(resolver.GetEnabledBuilder().Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(new []{ float3.zero, float3.zero, float3.zero, float3.zero, float3.zero }, Allocator.TempJob),
                Costs = new NativeArray<float>(new []{ 1f, 1f, 1f, 1f, 1f }, Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Equal(action11, action1);
        }

        [Test]
        public void Resolve_ShouldIncludeLocationHeuristics()
        {
            // Arrange
            var connection = new TestConnection("connection");
            var connection1 = new TestConnection("connection1");
            var connection2 = new TestConnection("connection2");
            
            // Act
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { connection }
            }.ToMock();
            var action1 = new TestAction("action1")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection1 }
            }.ToMock();
            var action11 = new TestAction("action11")
            {
                Effects = new IEffect[] { connection1 },
                Conditions = new ICondition[] { connection2 }
            }.ToMock();
            var action111 = new TestAction("action111")
            {
                Effects = new IEffect[] { connection2 },
            }.ToMock();
            var action2 = new TestAction("action2")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection2 }
            }.ToMock();
            
            var actions = new IConnectable[] { goal, action1, action2, action11, action111 };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            
            executableBuilder
                .SetExecutable(action111, true)
                .SetExecutable(action2, true);
            
            var positionBuilder = resolver.GetPositionBuilder();

            positionBuilder
                .SetPosition(goal, new float3(1, 0, 0))
                .SetPosition(action1, new float3(2, 0, 0))
                .SetPosition(action11, new float3(3, 0, 0))
                .SetPosition(action111, new float3(4, 0, 0))
                .SetPosition(action2, new float3(10, 0, 0)); // far away from goal
            
            var costBuilder = resolver.GetCostBuilder();
            var conditionBuilder = resolver.GetConditionBuilder();
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(resolver.GetEnabledBuilder().Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Equal(action111, action11, action1);
        }

        [Test]
        public void Resolve_ShouldIncludeCostHeuristics()
        {
            // Arrange
            var connection = new TestConnection("connection");
            var connection1 = new TestConnection("connection1");
            var connection2 = new TestConnection("connection2");
            
            // Act
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { connection }
            }.ToMock();
            var action1 = new TestAction("action1")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection1 }
            }.ToMock();
            var action11 = new TestAction("action11")
            {
                Effects = new IEffect[] { connection1 },
                Conditions = new ICondition[] { connection2 }
            }.ToMock();
            var action111 = new TestAction("action111")
            {
                Effects = new IEffect[] { connection2 },
            }.ToMock();
            var action2 = new TestAction("action2")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection2 }
            }.ToMock();
            
            var actions = new IConnectable[] { goal, action1, action2, action11, action111 };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            
            executableBuilder
                .SetExecutable(action111, true)
                .SetExecutable(action2, true);
            
            var positionBuilder = resolver.GetPositionBuilder();

            var costBuilder = resolver.GetCostBuilder();

            // Action 2 will be very expensive, other actions default to a cost of 1
            costBuilder.SetCost(action2, 100f);
            
            var conditionBuilder = resolver.GetConditionBuilder();
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(resolver.GetEnabledBuilder().Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Equal(action111, action11, action1);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_ShouldUseDistanceMultiplier(bool close)
        {
            var multiplier = close ? 1f : 0.2f;
            
            var rootConnection = new TestConnection("Root");
            var shortConnection = new TestConnection("Short");
            var longConnection = new TestConnection("Long");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { rootConnection }
            }.ToMock();
            var rootAction = new TestAction("action")
            {
                Effects = new IEffect[] { rootConnection },
                Conditions = new ICondition[] { shortConnection, longConnection }
            }.ToMock();
            var closeAction = new TestAction("closeAction")
            {
                Effects = new IEffect[] { shortConnection },
            }.ToMock();
            var farAction = new TestAction("farAction")
            {
                Effects = new IEffect[] { longConnection },
            }.ToMock();
            
            var actions = new IConnectable[] { goal, rootAction, closeAction, farAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Multiplier: 0.2
            // Far action: cost 1, distance 2 (10 * 0.2f) = 3
            // Close action: cost 4, distance 1 (5 * 0.2f) = 4
            
            // Multiplier: 1
            // Far action: cost 1, distance 10 (10 * 1) = 11
            // Close action: cost 4, distance 5 (5 * 1) = 9

            var executableBuilder = resolver.GetExecutableBuilder();
            
            executableBuilder
                .SetExecutable(closeAction, true)
                .SetExecutable(farAction, true);
            
            var positionBuilder = resolver.GetPositionBuilder();

            positionBuilder
                .SetPosition(rootAction, new Vector3(0f, 0f, 0f))
                .SetPosition(closeAction, new Vector3(5f, 0f, 0f))
                .SetPosition(farAction, new Vector3(10f, 0f, 0f));
            
            var costBuilder = resolver.GetCostBuilder();

            costBuilder
                .SetCost(closeAction, 4f)
                .SetCost(farAction, 1f);
            
            var conditionBuilder = resolver.GetConditionBuilder();
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(resolver.GetEnabledBuilder().Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = multiplier
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(2);
            
            if (close)
                result.Should().Equal(closeAction, rootAction);
            else
                result.Should().Equal(farAction, rootAction);
        }
        
        [Test]
        public void Resolve_ShouldNotResolve_CompletedCondition()
        {
            var rootConnection = new TestConnection("Root");
            var completedConnection = new TestConnection("Completed");
            var incompleteConnection = new TestConnection("Incomplete");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { rootConnection }
            }.ToMock();
            var rootAction = new TestAction("action")
            {
                Effects = new IEffect[] { rootConnection },
                Conditions = new ICondition[] { completedConnection, incompleteConnection }
            }.ToMock();
            var completedAction = new TestAction("completedAction")
            {
                Effects = new IEffect[] { completedConnection },
            }.ToMock();
            var incompleteAction = new TestAction("incompleteAction")
            {
                Effects = new IEffect[] { incompleteConnection },
            }.ToMock();
            
            var actions = new IConnectable[] { goal, rootAction, completedAction, incompleteAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            executableBuilder
                .SetExecutable(completedAction, true)
                .SetExecutable(incompleteAction, true);
            
            var positionBuilder = resolver.GetPositionBuilder();
            var costBuilder = resolver.GetCostBuilder();
            costBuilder.SetCost(completedAction, 1f);
            costBuilder.SetCost(incompleteAction, 100f);
            
            var conditionBuilder = resolver.GetConditionBuilder();

            conditionBuilder.SetConditionMet(completedConnection, true);
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(resolver.GetEnabledBuilder().Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Equal(incompleteAction, rootAction);
        }
        
        [Test]
        public void Resolve_ShouldNotResolve_ActionWithFalseConditionWithoutConnections()
        {
            var rootConnection = new TestConnection("Root");
            var availableConnection = new TestConnection("Available");
            var unavailableConnection = new TestConnection("Unavailable");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { rootConnection }
            }.ToMock();
            var expensiveAction = new TestAction("expensiveAction")
            {
                Effects = new IEffect[] { rootConnection },
            }.ToMock();
            
            var unavailableAction = new TestAction("subAction")
            {
                Effects = new IEffect[] { rootConnection },
                Conditions = new ICondition[] { unavailableConnection, availableConnection }
            }.ToMock();
            
            var shouldNotResolveAction = new TestAction("shouldNotResolveAction")
            {
                Effects = new IEffect[] { availableConnection },
            }.ToMock();
            
            var actions = new IConnectable[] { goal, expensiveAction, unavailableAction, shouldNotResolveAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            executableBuilder
                .SetExecutable(expensiveAction, true)
                .SetExecutable(unavailableAction, false)
                .SetExecutable(shouldNotResolveAction, true);
            
            var positionBuilder = resolver.GetPositionBuilder();
            var costBuilder = resolver.GetCostBuilder();
            costBuilder.SetCost(expensiveAction, 100f);
            
            var conditionBuilder = resolver.GetConditionBuilder();

            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(resolver.GetEnabledBuilder().Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(1);
            result.Should().Equal(expensiveAction);
        }
        
        [Test]
        public void Resolve_ShouldNotResolve_DisabledAction()
        {
            var rootConnection = new TestConnection("Root");
            var enabledConnection = new TestConnection("Enabled");
            var disabledConnection = new TestConnection("Disabled");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { rootConnection }
            }.ToMock();
            var rootAction = new TestAction("action")
            {
                Effects = new IEffect[] { rootConnection },
                Conditions = new ICondition[] { enabledConnection, disabledConnection }
            }.ToMock();
            var enabledAction = new TestAction("enabledAction")
            {
                Effects = new IEffect[] { enabledConnection },
            }.ToMock();
            var disabledAction = new TestAction("disabledAction")
            {
                Effects = new IEffect[] { disabledConnection },
            }.ToMock();
            
            var actions = new IConnectable[] { goal, rootAction, enabledAction, disabledAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            executableBuilder
                .SetExecutable(enabledAction, true)
                .SetExecutable(disabledAction, true);
            
            var positionBuilder = resolver.GetPositionBuilder();
            var costBuilder = resolver.GetCostBuilder();
            costBuilder.SetCost(disabledAction, 1f);
            costBuilder.SetCost(enabledAction, 100f);
            
            var conditionBuilder = resolver.GetConditionBuilder();
            var enabledBuilder = resolver.GetEnabledBuilder();

            enabledBuilder.SetEnabled(disabledAction, false);
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(enabledBuilder.Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Equal(enabledAction, rootAction);
        }
        
        [Test]
        public void Resolve_ShouldNotResolve_ChildOfDisabledAction()
        {
            var rootConnection = new TestConnection("Root");
            var enabledConnection = new TestConnection("Enabled");
            var disabledConnection = new TestConnection("Disabled");
            var disabledChildConnection = new TestConnection("DisabledChild");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { rootConnection }
            }.ToMock();
            var rootAction = new TestAction("action")
            {
                Effects = new IEffect[] { rootConnection },
                Conditions = new ICondition[] { enabledConnection, disabledConnection }
            }.ToMock();
            var enabledAction = new TestAction("enabledAction")
            {
                Effects = new IEffect[] { enabledConnection },
            }.ToMock();
            var disabledAction = new TestAction("disabledAction")
            {
                Effects = new IEffect[] { disabledConnection },
                Conditions = new ICondition[] { disabledChildConnection }
            }.ToMock();
            var disabledChildAction = new TestAction("disabledChildAction")
            {
                Effects = new IEffect[] { disabledChildConnection },
            }.ToMock();
            
            var actions = new IConnectable[] { goal, rootAction, enabledAction, disabledAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            executableBuilder
                .SetExecutable(enabledAction, true)
                .SetExecutable(disabledAction, true);
            
            var positionBuilder = resolver.GetPositionBuilder();
            var costBuilder = resolver.GetCostBuilder();
            costBuilder.SetCost(disabledAction, 1f);
            costBuilder.SetCost(disabledChildAction, 1f);
            costBuilder.SetCost(enabledAction, 100f);
            
            var conditionBuilder = resolver.GetConditionBuilder();
            var enabledBuilder = resolver.GetEnabledBuilder();

            enabledBuilder.SetEnabled(disabledAction, false);
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(enabledBuilder.Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Equal(enabledAction, rootAction);
        }

        [Test]
        public void Resolve_Should_UseDistanceCorrectly()
        {
            // Arrange
            var rootConnection = new TestConnection("Root");
            var farConnection = new TestConnection("Far");
            var closeConnection = new TestConnection("Close");
            
            var goal = new TestGoal("goal")
            {
                Conditions = new ICondition[] { rootConnection }
            }.ToMock();
            var rootAction = new TestAction("action")
            {
                Effects = new IEffect[] { rootConnection },
                Conditions = new ICondition[] { farConnection, closeConnection }
            }.ToMock();
            var farAction = new TestAction("farAction")
            {
                Effects = new IEffect[] { farConnection },
            }.ToMock();
            var closeAction = new TestAction("closeAction")
            {
                Effects = new IEffect[] { closeConnection },
            }.ToMock();
            
            var actions = new IConnectable[] { goal, rootAction, farAction, closeAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            executableBuilder
                .SetExecutable(farAction, true)
                .SetExecutable(closeAction, true);
            
            var positionBuilder = resolver.GetPositionBuilder();
            positionBuilder
                .SetPosition(rootAction, new Vector3(5f, 0f, 0f))
                .SetPosition(farAction, new Vector3(8f, 0f, 0f)) // 3 distance, but 8 from the player
                .SetPosition(closeAction, new Vector3(2f, 0f, 0f)); // 3 distance, but 2 from the player
            
            var costBuilder = resolver.GetCostBuilder();
            costBuilder
                .SetCost(rootAction, 1f)
                .SetCost(farAction, 1f)
                .SetCost(closeAction, 6f);
            
            var conditionBuilder = resolver.GetConditionBuilder();
            var enabledBuilder = resolver.GetEnabledBuilder();
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(enabledBuilder.Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });
            
            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();
            
            // Assert
            result.Should().HaveCount(2);
            result.Should().Equal(closeAction, rootAction);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_Should_HandleMultipleStartIndex(bool one)
        {
            // Arrange
            var rootConnection1 = new TestConnection("Root1");
            var rootConnection2 = new TestConnection("Root2");
            
            var goal1 = new TestGoal("goal1")
            {
                Conditions = new ICondition[] { rootConnection1 }
            }.ToMock();
            
            var goal2 = new TestGoal("goal2")
            {
                Conditions = new ICondition[] { rootConnection2 }
            }.ToMock();
            
            var rootAction1 = new TestAction("action1")
            {
                Effects = new IEffect[] { rootConnection1 },
            }.ToMock();
            
            var rootAction2 = new TestAction("action2")
            {
                Effects = new IEffect[] { rootConnection2 },
            }.ToMock();
            
            var actions = new IConnectable[] { goal1, goal2, rootAction1, rootAction2 };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            var executableBuilder = resolver.GetExecutableBuilder();
            executableBuilder
                .SetExecutable(rootAction1, true)
                .SetExecutable(rootAction2, true);
            
            var positionBuilder = resolver.GetPositionBuilder();
            positionBuilder
                .SetPosition(rootAction1, new Vector3(0f, 0f, 0f))
                .SetPosition(rootAction2, new Vector3(0f, 0f, 0f));
            
            var costBuilder = resolver.GetCostBuilder();
            costBuilder
                .SetCost(rootAction1, one ? 1f : 10f)
                .SetCost(rootAction2, one ? 10f : 1f);
            
            var conditionBuilder = resolver.GetConditionBuilder();
            var enabledBuilder = resolver.GetEnabledBuilder();
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = new NativeArray<int>(new [] { 0, 1 }, Allocator.TempJob),
                AgentPosition = Vector3.zero,
                IsEnabled = new NativeArray<bool>(enabledBuilder.Build(), Allocator.TempJob),
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob),
                Costs = new NativeArray<float>(costBuilder.Build(), Allocator.TempJob),
                ConditionsMet = new NativeArray<bool>(conditionBuilder.Build(), Allocator.TempJob),
                DistanceMultiplier = 1f
            });
            
            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();
            
            // Assert
            result.Should().HaveCount(1);
            result.Should().Equal(one ? rootAction1 : rootAction2);
        }
    }
}