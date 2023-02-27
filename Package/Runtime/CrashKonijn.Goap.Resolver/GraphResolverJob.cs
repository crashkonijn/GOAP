using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace CrashKonijn.Goap.Resolver
{
    public struct NodeData
    {
        public int Index;
        public int G;
        public int H;
        public int ParentIndex;
    
        public int F => this.G + this.H;
    }

    public struct RunData
    {
        public int StartIndex;
        public NativeArray<bool> IsExecutable;
    }

    public struct NodeSorter : IComparer<NodeData>
    {
        public int Compare(NodeData x, NodeData y)
        {
            return x.F.CompareTo(y.F);
        }
    }

    public struct GraphResolverJob : IJob
    {
        // Graph specific
        [ReadOnly] public NativeMultiHashMap<int, int> Connections;

        // Resolve specific
        [ReadOnly] public RunData RunData;

        // Results
        public NativeList<NodeData> Result;

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

            Debug.Log("ehmz");
            while (!openSet.IsEmpty)
            {
                var openList = openSet.GetValueArray(Allocator.Temp);
                openList.Sort(new NodeSorter());
            
                var currentNode = openList[0];
                Debug.Log("Checking node: " + currentNode.Index + "");

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

            Debug.Log("Done");
            openSet.Dispose();
            closedSet.Dispose();
        }

        private int Heuristic(int currentIndex, int previousIndex)
        {
            return 0;
        }

        private void RetracePath(NodeData startNode, NativeHashMap<int, NodeData> closedSet, NativeList<NodeData> path)
        {
            var currentNode = startNode;
            while (currentNode.ParentIndex != -1)
            {
                path.Add(currentNode);
                currentNode = closedSet[currentNode.ParentIndex];
            }
            // path.Reverse();
        }
    }
}