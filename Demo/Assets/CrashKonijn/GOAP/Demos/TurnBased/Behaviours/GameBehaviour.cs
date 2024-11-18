using CrashKonijn.Goap.Core;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class GameBehaviour : MonoBehaviour
    {
        private IGrid grid;
        private TurnBehaviour turns;
        private IGoap goapRunner;

        [SerializeField]
        private GameObject agentPrefab;


        private void Awake()
        {
            this.grid = FindObjectOfType<GridBehaviour>();
            this.goapRunner = FindObjectOfType<GoapBehaviour>();
            this.turns = this.GetComponent<TurnBehaviour>();
        }

        private void Start()
        {
            this.SpawnAgent("TurnBased", Color.red);
            this.SpawnAgent("TurnBased", Color.blue);
        }

        private void SpawnAgent(string setId, Color color)
        {
            var actionProvider = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<GoapActionProvider>();

            actionProvider.AgentType = this.goapRunner.GetAgentType(setId);
            actionProvider.gameObject.SetActive(true);

            actionProvider.gameObject.transform.name = $"agent {actionProvider.GetInstanceID()}";

            var spriteRenderer = actionProvider.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = color;

            this.turns.Register(actionProvider);
        }

        private Vector3 GetRandomPosition()
        {
            var tile = this.grid.GetWalkableTiles().Random();

            return new Vector3(tile.X, 0.5f, tile.Y);
        }
    }
}
