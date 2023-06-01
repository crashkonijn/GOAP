using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Resolver.Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver
{
    public class PositionBuilder : IPositionBuilder
    {
        private readonly List<IAction> actionIndexList;
        private float3[] executableList;

        public PositionBuilder(List<IAction> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.executableList = this.actionIndexList.Select(x => GraphResolverJob.InvalidPosition).ToArray();
        }
        
        public IPositionBuilder SetPosition(IAction action, Vector3 position)
        {
            var index = this.actionIndexList.IndexOf(action);

            if (index == -1)
                return this;
            
            this.executableList[index] = position;

            return this;
        }
        
        public float3[] Build()
        {
            return this.executableList;
        }

        public void Clear()
        {
            this.executableList = this.actionIndexList.Select(x => GraphResolverJob.InvalidPosition).ToArray();
        }
    }
}