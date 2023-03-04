using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
using CrashKonijn.Goap.UnitTests.Data;
using FluentAssertions;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;

namespace CrashKonijn.Goap.UnitTests
{
    public class GraphResolverTests
    {
        private class TestAction : IAction
        {
            public string Name { get; }
            public Guid Guid { get; } = Guid.NewGuid();
            public IEffect[] Effects { get; set; } = {};
            public ICondition[] Conditions { get; set; } = {};

            public TestAction(string name)
            {
                this.Name = name;
            }
        }
        
        [Test]
        public void Resolve_WithNoActions_ReturnsEmptyList()
        {
            // Arrange
            var actions = Array.Empty<IAction>();
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(new [] { false }, Allocator.TempJob),
                Positions = new NativeArray<float3>(new [] { float3.zero }, Allocator.TempJob)
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
            var goal = new TestAction("goal")
            {
                Conditions = new ICondition[] { connection }
            };
            var action = new TestAction("action")
            {
                Effects = new IEffect[] { connection }
            };
            
            var actions = new IAction[] { goal, action };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(new [] { false, true }, Allocator.TempJob),
                Positions = new NativeArray<float3>(new []{ float3.zero, float3.zero }, Allocator.TempJob)
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
            
            var goal = new TestAction("goal")
            {
                Conditions = new ICondition[] { connection }
            };
            var firstAction = new TestAction("action1")
            {
                Effects = new IEffect[] { connection }
            };
            var secondAction = new TestAction("action2")
            {
                Effects = new IEffect[] { connection }
            };
            var thirdAction = new TestAction("action3")
            {
                Effects = new IEffect[] { connection }
            };
            
            var actions = new IAction[] { goal, firstAction, secondAction, thirdAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(new [] { false, false, false, true }, Allocator.TempJob),
                Positions = new NativeArray<float3>(new []{ float3.zero, float3.zero, float3.zero, float3.zero }, Allocator.TempJob)
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
            
            var goal = new TestAction("goal")
            {
                Conditions = new ICondition[] { connection }
            };
            var action1 = new TestAction("action1")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection2 }
            };
            var action2 = new TestAction("action2")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection3 }
            };
            var action11 = new TestAction("action11")
            {
                Effects = new IEffect[] { connection2 }
            };
            var action22 = new TestAction("action22")
            {
                Effects = new IEffect[] { connection3 }
            };
            
            var actions = new IAction[] { goal, action1, action2, action11, action22 };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            var executableBuilder = resolver.GetExecutableBuilder();

            executableBuilder.SetExecutable(action11, true);
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(new []{ float3.zero, float3.zero, float3.zero, float3.zero, float3.zero }, Allocator.TempJob)
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
            var goal = new TestAction("goal")
            {
                Conditions = new ICondition[] { connection }
            };
            var action1 = new TestAction("action1")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection1 }
            };
            var action11 = new TestAction("action11")
            {
                Effects = new IEffect[] { connection1 },
                Conditions = new ICondition[] { connection2 }
            };
            var action111 = new TestAction("action111")
            {
                Effects = new IEffect[] { connection2 },
            };
            var action2 = new TestAction("action2")
            {
                Effects = new IEffect[] { connection },
                Conditions = new ICondition[] { connection2 }
            };
            
            var actions = new IAction[] { goal, action1, action2, action11, action111 };
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
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob),
                Positions = new NativeArray<float3>(positionBuilder.Build(), Allocator.TempJob)
            });

            var result = handle.Complete();
            
            // Cleanup
            resolver.Dispose();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Equal(action111, action11, action1);
        }
    }
}