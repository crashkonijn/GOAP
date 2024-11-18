using System.Linq;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class GridBehaviour : MonoBehaviour, IGrid
    {
        public Transform tilePrefab;
        public int width = 10;
        public int height = 10;
        private ITile[,] grid;
        private ITile[] tiles;
        private IPathfinding pathfinding;

        private void Awake()
        {
            this.grid = new ITile[this.width, this.height];
            this.tiles = new ITile[this.width * this.height];
            this.pathfinding = this.GetComponent<PathfindingBehaviour>();
            
            for (var x = 0; x < this.width; x++)
            {
                for (var y = 0; y < this.height; y++)
                {
                    var tile = this.CreateTile(x, y);
                    this.grid[x, y] = tile;
                    this.tiles[x + y * this.width] = tile;
                }
            }
        }

        private ITile CreateTile(int x, int y)
        {
            var instance = Instantiate(this.tilePrefab, this.transform, true);
            instance.name = $"Tile {x} {y}";
            instance.position = new Vector3(x, 0, y);
            
            var tile = instance.GetComponent<ITile>();

            tile.Initialize(x, y, this);
            tile.SetWalkable(Random.Range(0, 100) >= 10);
            
            return tile;
        }

        public ITile[,] GetGrid()
        {
            return this.grid;
        }

        public ITile[] GetWalkableTiles()
        {
            return this.tiles.Where(x => x.IsWalkable).ToArray();
        }

        public ITile GetTile(Vector3 position)
        {
            return this.grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z)];
        }

        public ITile[] GetPath(ITile from, ITile to)
        {
            return this.pathfinding.FindPath(this.grid, from, to);
        }

        public ITile[] GetPath(Vector3 from, Vector3 to)
        {
            return this.pathfinding.FindPath(this.grid, this.GetTile(from), this.GetTile(to));
        }
    }
}
