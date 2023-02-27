using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.UnitTests.Data;
using FluentAssertions;
using LamosInteractive.Goap.Interfaces;
using NUnit.Framework;
using Unity.Collections;

namespace CrashKonijn.Goap.UnitTests
{
    public class GraphResolverTests
    {
        private class TestAction : IAction
        {
            public string Name { get; }
            public Guid Guid { get; } = Guid.NewGuid();
            public List<IEffect> Effects { get; set; } = new();
            public List<ICondition> Conditions { get; set; } = new();

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
                IsExecutable = new NativeArray<bool>(new [] { false }, Allocator.TempJob)
            });

            var result = handle.Complete();

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
                Conditions = new List<ICondition>(new [] { connection })
            };
            var action = new TestAction("action")
            {
                Effects = new List<IEffect>(new [] { connection })
            };
            
            var actions = new IAction[] { goal, action };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(new [] { false, true }, Allocator.TempJob)
            });

            var result = handle.Complete();

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
                Conditions = new List<ICondition>(new [] { connection })
            };
            var firstAction = new TestAction("action1")
            {
                Effects = new List<IEffect>(new [] { connection })
            };
            var secondAction = new TestAction("action2")
            {
                Effects = new List<IEffect>(new [] { connection })
            };
            var thirdAction = new TestAction("action3")
            {
                Effects = new List<IEffect>(new [] { connection })
            };
            
            var actions = new IAction[] { goal, firstAction, secondAction, thirdAction };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(new [] { false, false, false, true }, Allocator.TempJob)
            });

            var result = handle.Complete();

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
                Conditions = new List<ICondition>(new [] { connection })
            };
            var action1 = new TestAction("action1")
            {
                Effects = new List<IEffect>(new [] { connection }),
                Conditions = new List<ICondition>(new [] { connection2 })
            };
            var action2 = new TestAction("action2")
            {
                Effects = new List<IEffect>(new [] { connection }),
                Conditions = new List<ICondition>(new [] { connection3 })
            };
            var action11 = new TestAction("action11")
            {
                Effects = new List<IEffect>(new [] { connection2 })
            };
            var action22 = new TestAction("action22")
            {
                Effects = new List<IEffect>(new [] { connection3 })
            };
            
            var actions = new IAction[] { goal, action1, action2, action11, action22 };
            var resolver = new GraphResolver(actions, new TestKeyResolver());
            var executableBuilder = resolver.GetExecutableBuilder();

            executableBuilder.SetExecutable(action11, true);
            
            // Act
            var handle = resolver.StartResolve(new RunData
            {
                StartIndex = 0,
                IsExecutable = new NativeArray<bool>(executableBuilder.Build(), Allocator.TempJob)
            });

            var result = handle.Complete();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Equal(action11, action1);
            
            // Cleanup
            resolver.Dispose();
        }
    }
}