using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core;
using Unity.Mathematics;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver
{
    public class PositionBuilder : IPositionBuilder
    {
        private readonly List<IConnectable> actionIndexList;
        private float3[] executableList;

        public PositionBuilder(List<IConnectable> actionIndexList)
        {
            this.actionIndexList = actionIndexList;
            this.executableList = this.actionIndexList.Select(x => GraphResolverJob.InvalidPosition).ToArray();
        }

        public IPositionBuilder SetPosition(IConnectable action, Vector3? position)
        {
            var index = this.GetIndex(action);

            if (index == -1)
                return this;

            this.executableList[index] = position ?? GraphResolverJob.InvalidPosition;

            return this;
        }

        private int GetIndex(IConnectable condition)
        {
            for (var i = 0; i < this.actionIndexList.Count; i++)
            {
                if (this.actionIndexList[i] == condition)
                    return i;
            }

            return -1;
        }

        public float3[] Build()
        {
            return this.executableList;
        }

        public void Clear()
        {
            for (var i = 0; i < this.executableList.Length; i++)
            {
                this.executableList[i] = GraphResolverJob.InvalidPosition;
            }
        }
    }
}