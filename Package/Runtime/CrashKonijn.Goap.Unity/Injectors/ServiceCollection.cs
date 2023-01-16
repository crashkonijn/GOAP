using System;
using System.Collections.Generic;
using System.Linq;

namespace CrashKonijn.Goap.Unity.Injectors
{
    public class ServiceCollection
    {
        private readonly Dictionary<Type, IInstance> setup = new();
        private readonly Dictionary<Type, object> instances = new();

        public void AddSingleton<TInterface, TType>()
            where TType : TInterface
        {
            this.setup.Add(typeof(TInterface), new Singleton(typeof(TType)));
        }

        public void AddTransient<TInterface, TType>()
            where TType : TInterface
        {
            this.setup.Add(typeof(TInterface), new Transient(typeof(TType)));
        }
        
        private object[] GetParameters(Type type)
        {
            var ctor = type.GetConstructors().SingleOrDefault();
            if (ctor == null)
                throw new Exception($"Type '{type.Name}' has no constructor.");

            var parameters = ctor.GetParameters()
                .Select(
                    parameter => this.GetInstance(parameter.ParameterType)
                ).ToArray();

            return parameters;
        }

        public T Get<T>()
            where T : class
        {
            return (T) this.GetInstance(typeof(T));
        }

        private object GetInstance(Type type)
        {
            var instance = this.setup[type];

            return instance switch
            {
                Singleton singleton => this.GetSingleton(type, singleton),
                Transient transient => this.CreateInstance(transient.Get()),
                _ => throw new Exception("This should not happen")
            };
        }

        private object GetSingleton(Type type, Singleton instance)
        {
            if (!this.instances.ContainsKey(type))
            {
                this.instances.Add(type, this.CreateInstance(instance.Get()));
            }

            return this.instances[type];
        }
        
        private object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type, args:this.GetParameters(type));
        }
    }

    internal class Singleton : IInstance
    {
        private readonly Type type;

        public Singleton(Type type)
        {
            this.type = type;
        }
        
        public Type Get() => this.type;
    }

    internal class Transient : IInstance
    {
        private readonly Type type;

        public Transient(Type type)
        {
            this.type = type;
        }
        
        public Type Get() => this.type;
    }

    internal class Instance<T>
    {
        private readonly T instance;

        public Instance(T instance)
        {
            this.instance = instance;
        }
        
        public T Get() => this.instance;
    }

    internal interface IInstance
    {
        public Type Get();
    }
}