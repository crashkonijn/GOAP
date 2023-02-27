using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace CrashKonijn.Goap.Resolver
{
    [BurstCompile]
    public struct NodeData
    {
        public int Index;
        public int G;
        public int H;
        public int ParentIndex;
    
        public int F => this.G + this.H;
    }

    [BurstCompile]
    public struct RunData
    {
        public int StartIndex;
        public NativeArray<bool> IsExecutable;
        public NativeArray<float3> Positions;
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
        [ReadOnly] public NativeMultiHashMap<int, int> Connections;

        // Resolve specific
        [ReadOnly] public RunData RunData;

        // Results
        public NativeList<NodeData> Result;
        
        public static readonly float3 InvalidPosition = new float3(float.MaxValue, float.MaxValue, float.MaxValue);

        [BurstCompile]
        public void Execute()
        {
            var nodeCount = this.Connections.Count();
            var runData = this.RunData;
        
            var openSet = new NativeHashMap<int, NodeData>(nodeCount, Allocator.Temp);
            var closedSet = new NativeHashMap<int, NodeData>(nodeCount, Allocator.Temp);
        
            var nodeData = new NodeData
            {
                Index = runData.StartIndex,
                G = 0,
                H = int.MaxValue,
                ParentIndex = -1
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
            
                foreach (var neighborIndex in this.Connections.GetValuesForKey(currentNode.Index))
                {
                    if (closedSet.ContainsKey(neighborIndex))
                    {
                        continue;
                    }
                
                    var newG = currentNode.G + 1;
                    NodeData neighbor;
                
                    if (!openSet.TryGetValue(neighborIndex, out neighbor))
                    {
                        neighbor = new NodeData
                        {
                            Index = neighborIndex,
                            G = newG,
                            H = this.Heuristic(neighborIndex, currentNode.Index),
                            ParentIndex = currentNode.Index
                        };
                        openSet.Add(neighborIndex, neighbor);
                        continue;
                    }
                
                    if (newG < neighbor.G)
                    {
                        neighbor.G = newG;
                        neighbor.ParentIndex = currentNode.Index;
                    
                        openSet.Remove(neighborIndex);
                        openSet.Add(neighborIndex, neighbor);
                    }
                }

                openList.Dispose();
            }

            openSet.Dispose();
            closedSet.Dispose();
        }

        private int Heuristic(int currentIndex, int previousIndex)
        {
            var previousPosition = this.RunData.Positions[previousIndex];
            var currentPosition = this.RunData.Positions[currentIndex];

            if (previousPosition.Equals(InvalidPosition) || currentPosition.Equals(InvalidPosition))
            {
                return 0;
            }

            return (int) math.ceil(math.distance(previousPosition, currentPosition));
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
    }
}