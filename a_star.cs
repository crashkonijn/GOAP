using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public struct NodeData
{
    public int Index;
    public int G;
    public int H;
    public int ParentIndex;
}

public struct FindPathJob : IJob
{
    [ReadOnly] public NativeArray<int> Connections;
    [ReadOnly] public NativeArray<bool> Map;
    [ReadOnly] public int StartIndex;
    [ReadOnly] public int EndIndex;
    [WriteOnly] public NativeList<NodeData> Path;

    public void Execute()
    {
        var openSet = new NativeMinHeap();
        var closedSet = new NativeHashMap<int, NodeData>(Map.Length, Allocator.Temp);
        var nodeData = new NodeData
        {
            Index = StartIndex,
            G = 0,
            H = Heuristic(StartIndex, EndIndex),
            ParentIndex = -1
        };
        openSet.Add(nodeData);

        while (openSet.Length > 0)
        {
            var currentNode = openSet.Pop();

            if (currentNode.Index == EndIndex)
            {
                RetracePath(nodeData, closedSet, Path);
                break;
            }

            closedSet.TryAdd(currentNode.Index, currentNode);

            int connectionStart = Connections[currentNode.Index];
            int connectionEnd = Connections[currentNode.Index + 1];
            for (int i = connectionStart; i < connectionEnd; i++)
            {
                int neighborIndex = Connections[i];
                if (Map[neighborIndex])
                {
                    continue;
                }

                if (closedSet.ContainsKey(neighborIndex))
                {
                    continue;
                }

                int newG = currentNode.G + 1;
                NodeData neighbor;
                if (!openSet.TryGetValue(neighborIndex, out neighbor))
                {
                    neighbor = new NodeData
                    {
                        Index = neighborIndex,
                        G = newG,
                        H = Heuristic(neighborIndex, EndIndex),
                        ParentIndex = currentNode.Index
                    };
                    openSet.Add(neighbor);
                }
                else if (newG < neighbor.G)
                {
                    neighbor.G = newG;
                    neighbor.ParentIndex = currentNode.Index;
                    openSet.UpdateItem(neighbor);
                }
            }
        }

        openSet.Dispose();
        closedSet.Dispose();
    }

    private int Heuristic(int index, int endIndex)
    {
        return 0;
    }

    private void RetracePath(NodeData startNode, NativeHashMap<int, NodeData> closedSet, NativeList<NodeData> path)
    {
        NodeData currentNode = startNode;
        while (currentNode.ParentIndex != -1)
        {
            path.Add(currentNode);
            currentNode = closedSet[currentNode.ParentIndex];
        }
        path.Reverse();
    }
}