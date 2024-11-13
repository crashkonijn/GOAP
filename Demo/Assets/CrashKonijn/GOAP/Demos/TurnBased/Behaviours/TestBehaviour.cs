using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TestBehaviour : MonoBehaviour
    {
        private IGrid grid;
        private IPathfinding pathfinding;
        private ITile[] path;

        private void Awake()
        {
            this.grid = FindObjectOfType<GridBehaviour>();
            this.pathfinding = FindObjectOfType<PathfindingBehaviour>();
        }

        private void Start()
        {
            this.path = this.GetPath();
        }

        private ITile[] GetPath()
        {
            var tiles = this.grid.GetWalkableTiles();
            var from = tiles.Random();
            var to = tiles.Random();

            return this.pathfinding.FindPath(this.grid.GetGrid(), from, to);
        }

        private void OnDrawGizmos()
        {
            if (this.path == null)
                return;

            Gizmos.color = Color.blue;
            foreach (var tile in this.path)
            {
                Gizmos.DrawSphere(new Vector3(tile.X, 0, tile.Y), 0.5f);
            }
        }
    }
}
