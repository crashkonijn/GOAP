using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver
{
    [BurstCompile]
    public struct NodeData
    {
        public int Index;
        public float G;
        public float H;
        public int ParentIndex;
        public float3 Position;
    
        public float F => this.G + this.H;
    }

    [BurstCompile]
    public struct RunData
    {
        public int StartIndex;
        public float3 StartPosition;
        // Index = NodeIndex
        public NativeArray<bool> IsEnabled;
        public NativeArray<bool> IsExecutable;
        // Index = ConditionIndex
        public NativeArray<bool> ConditionsMet;
        public NativeArray<float3> Positions;
        public NativeArray<float> Costs;
        public float DistanceMultiplier;
    }

    [BurstCompile]
    public struct NodeSorter : IComparer<NodeData>
    {
        public int Compare(NodeData x, NodeData y)
        {
            return x.F.CompareTo(y.F);
        }
    }

    [BurstCompile]
    public struct GraphResolverJob : IJob
    {
        // Graph specific
#if UNITY_COLLECTIONS_2_1
        // Dictionary<ActionIndex, ConditionIndex[]>
        [ReadOnly] public NativeParallelMultiHashMap<int, int> NodeConditions;
        // Dictionary<ConditionIndex, NodeIndex[]>
        [ReadOnly] public NativeParallelMultiHashMap<int, int> ConditionConnections;
#else
        // Dictionary<ActionIndex, ConditionIndex[]>
        [ReadOnly] public NativeMultiHashMap<int, int> NodeConditions;
        // Dictionary<ConditionIndex, NodeIndex[]>
        [ReadOnly] public NativeMultiHashMap<int, int> ConditionConnections;
#endif

        // Resolve specific
        [ReadOnly] public RunData RunData;

        // Results
        public NativeList<NodeData> Result;
        
        public static readonly float3 InvalidPosition = new float3(float.MaxValue, float.MaxValue, float.MaxValue);

        [BurstCompile]
        public void Execute()
        {
            var nodeCount = this.NodeConditions.Count();
            var runData = this.RunData;
        
            var openSet = new NativeHashMap<int, NodeData>(nodeCount, Allocator.Temp);
            var closedSet = new NativeHashMap<int, NodeData>(nodeCount, Allocator.Temp);
        
            var nodeData = new NodeData
            {
                Index = runData.StartIndex,
                G = 0,
                H = int.MaxValue,
                ParentIndex = -1,
                Position = runData.StartPosition
            };
            openSet.Add(runData.StartIndex, nodeData);

            while (!openSet.IsEmpty)
            {
                var openList = openSet.GetValueArray(Allocator.Temp);
                openList.Sort(new NodeSorter());
            
                var currentNode = openList[0];

                if (runData.IsExecutable[currentNode.Index])
                {
                    this.RetracePath(currentNode, closedSet, this.Result);
                    break;
                }

                closedSet.TryAdd(currentNode.Index, currentNode);
                openSet.Remove(currentNode.Index);
                
                // If this node has a condition that is false and has no connections, it is unresolvable
                if (this.HasUnresolvableCondition(currentNode.Index))
                {
                    continue;
                }

                foreach (var conditionIndex in this.NodeConditions.GetValuesForKey(currentNode.Index))
                {
                    if (runData.ConditionsMet[conditionIndex])
                    {
                        continue;
                    }
                    
                    foreach (var neighborIndex in this.ConditionConnections.GetValuesForKey(conditionIndex))
                    {
                        if (closedSet.ContainsKey(neighborIndex))
                        {
                            continue;
                        }
                        
                        if (!runData.IsEnabled[neighborIndex])
                        {
                            continue;
                        }
                
                        var neighborPosition = this.GetPosition(currentNode, neighborIndex);
                        
                        var newG = this.GetNewCost(currentNode, neighborIndex, neighborPosition);
                        NodeData neighbor;
                
                        // Current neighbour is not in the open set
                        if (!openSet.TryGetValue(neighborIndex, out neighbor))
                        {
                            neighbor = new NodeData
                            {
                                Index = neighborIndex,
                                G = newG,
                                H = this.GetHeuristic(neighborIndex),
                                ParentIndex = currentNode.Index,
                                Position = neighborPosition
                            };
                            openSet.Add(neighborIndex, neighbor);
                            continue;
                        }
                
                        // This neighbour has a lower cost
                        if (newG < neighbor.G)
                        {
                            neighbor.G = newG;
                            neighbor.ParentIndex = currentNode.Index;
                            neighbor.Position = neighborPosition;
                    
                            openSet.Remove(neighborIndex);
                            openSet.Add(neighborIndex, neighbor);
                        }
                    }
                }

                openList.Dispose();
            }

            openSet.Dispose();
            closedSet.Dispose();
        }
        
        private float GetNewCost(NodeData currentNode, int neighborIndex, float3 neighborPosition)
        {
            return currentNode.G + this.RunData.Costs[neighborIndex] + this.GetDistanceCost(currentNode, neighborPosition);
        }

        private float GetHeuristic(int neighborIndex)
        {
            return this.AmountOfUnmetConditions(neighborIndex);
        }

        private float GetDistanceCost(NodeData previousNode, float3 currentPosition)
        {
            var previousPosition = previousNode.Position;

            if (previousPosition.Equals(InvalidPosition) || currentPosition.Equals(InvalidPosition))
            {
                return 0f;
            }

            return math.distance(previousPosition, currentPosition) * this.RunData.DistanceMultiplier;
        }

        private float3 GetPosition(NodeData currentNode, int currentIndex)
        {
            var pos = this.RunData.Positions[currentIndex];
            
            if (pos.Equals(InvalidPosition))
                return currentNode.Position;

            return pos;
        }

        private void RetracePath(NodeData startNode, NativeHashMap<int, NodeData> closedSet, NativeList<NodeData> path)
        {
            var currentNode = startNode;
            while (currentNode.ParentIndex != -1)
            {
                path.Add(currentNode);
                currentNode = closedSet[currentNode.ParentIndex];
            }
        }

        private bool HasUnresolvableCondition(int currentIndex)
        {
            foreach (var conditionIndex in this.NodeConditions.GetValuesForKey(currentIndex))
            {
                if (this.RunData.ConditionsMet[conditionIndex])
                {
                    continue;
                }
                    
                if (!this.ConditionConnections.GetValuesForKey(conditionIndex).MoveNext())
                {
                    return true;
                }
            }

            return false;
        }
        
        private int AmountOfUnmetConditions(int currentIndex)
        {
            var count = 0;
            foreach (var conditionIndex in this.NodeConditions.GetValuesForKey(currentIndex))
            {
                if (!this.RunData.ConditionsMet[conditionIndex])
                {
                    count++;
                }
            }

            return count;
        }
    }
}