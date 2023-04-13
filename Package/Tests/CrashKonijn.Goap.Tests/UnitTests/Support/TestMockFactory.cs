using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;

namespace CrashKonijn.Goap.UnitTests.Support
{
    public class TestMockFactory
    {
        private readonly Dictionary<Type, object> mocks = new();

        public T Create<T>() where T : class
        {
            if (this.mocks.ContainsKey(typeof(T)))
                return this.Get<T>();
            
            if (typeof(T).IsInterface)
            {
                this.CreateInterface<T>();
                return this.Get<T>();
            }

            this.CreateClass<T>();
            return this.Get<T>();
        }

        private void CreateInterface<T>() where T : class
        {
            this.mocks.Add(typeof(T), Substitute.For<T>());
        }

        private void CreateClass<T>() where T : class
        {
            this.mocks.Add(typeof(T), Substitute.For<T>(this.GetParameters<T>()));
        }

        private object[] GetParameters<T>() where T : class
        {
            var ctor = typeof(T).GetConstructors().SingleOrDefault();
            if (ctor == null)
                throw new Exception($"Type '{typeof(T).Name}' has no constructor.");

            var parameters = ctor.GetParameters()
                .Select(
                    parameter => this.mocks.ContainsKey(parameter.ParameterType)
                        ? this.mocks[parameter.ParameterType]
                        : null
                ).ToArray();

            return parameters;
        }

        public void Setup<T>() where T : class
        {
            var ctor = typeof(T).GetConstructors().SingleOrDefault();

            if (ctor == null)
                return;

            ctor.GetParameters().ToList().ForEach(this.MockParameter);

            this.Create<T>();
        }

        private void MockParameter(ParameterInfo param)
        {
            var method = this.GetType().GetMethod(nameof(this.Create));
            var generic = method.MakeGenericMethod(param.ParameterType);
            generic.Invoke(this, null);
        }

        public T Get<T>() where T : class
        {
            if (this.mocks.ContainsKey(typeof(T)))
                return (T)this.mocks[typeof(T)];

            return this.Create<T>();
        }
    }
}