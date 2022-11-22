using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolvers;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class KeyResolverBehaviour : MonoBehaviour, IKeyResolver
    {
        private readonly KeyResolver resolver = new();

        public string GetKey(IAction action, ICondition condition) => this.resolver.GetKey(action, condition);
        public string GetKey(IAction action, IEffect effect) => this.resolver.GetKey(action, effect);
        public void SetWorldData(IWorldData globalWorldData) => this.resolver.SetWorldData(globalWorldData);
    }
}