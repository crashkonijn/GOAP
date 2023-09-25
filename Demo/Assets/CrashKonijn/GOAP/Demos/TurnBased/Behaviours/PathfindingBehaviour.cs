using System;
using System.Collections.Generic;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    // Note: This is very inefficient. Please use a better pathfinding algorithm in your own projects. 
    public class PathfindingBehaviour : MonoBehaviour, IPathfinding
    {
        public ITile[] FindPath(ITile[,] grid, ITile startTile, ITile targetTile)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);

            // Create a priority queue for open nodes
            var openNodes = new PriorityQueue<ITile>();
            openNodes.Enqueue(startTile, 0);

            // Store the cost to reach each tile from the start tile
            var gScore = new Dictionary<ITile, float>();
            gScore[startTile] = 0;

            // Store the previous tile in the best-known path
            var cameFrom = new Dictionary<ITile, ITile>();

            // Calculate the heuristic distance between two tiles (Manhattan distance)
            float HeuristicDistance(ITile a, ITile b)
            {
                return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            }

            while (openNodes.Count > 0)
            {
                var currentTile = openNodes.Dequeue();

                if (currentTile == targetTile)
                {
                    // Found the target tile, reconstruct the path and return it
                    return ReconstructPath(cameFrom, currentTile);
                }

                foreach (var neighbor in GetNeighbors(grid, currentTile, width, height))
                {
                    // Calculate the tentative gScore from start to the neighbor
                    var tentativeGScore = gScore[currentTile] + 1; // Assuming tiles have a cost of 1

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        // This path to the neighbor is better than any previous one, so record it
                        cameFrom[neighbor] = currentTile;
                        gScore[neighbor] = tentativeGScore;

                        // Calculate the fScore for the neighbor (fScore = gScore + heuristic)
                        var fScore = tentativeGScore + HeuristicDistance(neighbor, targetTile);

                        if (!openNodes.Contains(neighbor))
                        {
                            // Add the neighbor to the open nodes
                            openNodes.Enqueue(neighbor, fScore);
                        }
                    }
                }
            }

            // No path found
            return null;
        }

        private static ITile[] ReconstructPath(Dictionary<ITile, ITile> cameFrom, ITile currentTile)
        {
            var path = new List<ITile>() { currentTile };

            while (cameFrom.ContainsKey(currentTile))
            {
                currentTile = cameFrom[currentTile];
                path.Add(currentTile);
            }

            path.Reverse();
            return path.ToArray();
        }

        private static IEnumerable<ITile> GetNeighbors(ITile[,] grid, ITile tile, int width, int height)
        {
            var neighbors = new List<ITile>();

            // Get the adjacent tiles (up, down, left, right)
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            for (var i = 0; i < dx.Length; i++)
            {
                var nx = tile.X + dx[i];
                var ny = tile.Y + dy[i];

                if (nx >= 0 && nx < width && ny >= 0 && ny < height && grid[nx, ny].IsWalkable)
                {
                    neighbors.Add(grid[nx, ny]);
                }
            }

            return neighbors;
        }
    }

// A simple priority queue implementation using a min heap
    public class PriorityQueue<T>
    {
        private List<(T item, float priority)> elements = new List<(T, float)>();

        public int Count => this.elements.Count;

        public void Enqueue(T item, float priority)
        {
            this.elements.Add((item, priority));
            var index = this.elements.Count - 1;

            while (index > 0)
            {
                var parentIndex = (index - 1) / 2;
                if (this.elements[parentIndex].priority <= priority)
                    break;

                (var tempItem, var tempPriority) = this.elements[parentIndex];
                this.elements[parentIndex] = this.elements[index];
                this.elements[index] = (tempItem, tempPriority);

                index = parentIndex;
            }
        }

        public T Dequeue()
        {
            var item = this.elements[0].item;
            this.elements[0] = this.elements[this.elements.Count - 1];
            this.elements.RemoveAt(this.elements.Count - 1);

            var index = 0;
            while (true)
            {
                var leftChildIndex = 2 * index + 1;
                var rightChildIndex = 2 * index + 2;
                var smallestChildIndex = index;

                if (leftChildIndex < this.elements.Count && this.elements[leftChildIndex].priority < this.elements[smallestChildIndex].priority)
                    smallestChildIndex = leftChildIndex;

                if (rightChildIndex < this.elements.Count && this.elements[rightChildIndex].priority < this.elements[smallestChildIndex].priority)
                    smallestChildIndex = rightChildIndex;

                if (smallestChildIndex == index)
                    break;

                (var tempItem, var tempPriority) = this.elements[index];
                this.elements[index] = this.elements[smallestChildIndex];
                this.elements[smallestChildIndex] = (tempItem, tempPriority);

                index = smallestChildIndex;
            }

            return item;
        }

        public bool Contains(T item)
        {
            foreach ((var currentItem, _) in this.elements)
            {
                if (currentItem.Equals(item))
                    return true;
            }
            return false;
        }
    }
}