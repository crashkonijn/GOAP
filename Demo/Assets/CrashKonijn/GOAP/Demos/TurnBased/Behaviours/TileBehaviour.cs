using System;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TileBehaviour : MonoBehaviour, ITile
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsWalkable { get; private set; }
        public Vector3 Position { get; private set; }
        
        private IGrid grid;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public void Initialize(int x, int y, IGrid grid)
        {
            this.X = x;
            this.Y = y;
            this.grid = grid;
            this.IsWalkable = true;
            this.Position = new Vector3(x, 0f, y);
            this.UpdateColor();
        }

        public void SetWalkable(bool isWalkable)
        {
            this.IsWalkable = isWalkable;
            this.UpdateColor();
        }

        private void UpdateColor()
        {
            this.spriteRenderer.color = this.GetColor();
        }

        private Color GetColor()
        {
            if (!this.IsWalkable)
                return Color.red;
            
            return Color.green;
        }
    }
}