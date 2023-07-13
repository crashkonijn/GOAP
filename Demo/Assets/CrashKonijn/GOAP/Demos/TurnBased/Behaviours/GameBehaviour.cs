using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using CrashKonijn.Goap.Interfaces;
using Demos;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class GameBehaviour : MonoBehaviour
    {
        private IGrid grid;
        private TurnBehaviour turns;
        private IGoapRunner goapRunner;
        
        [SerializeField]
        private GameObject agentPrefab;


        private void Awake()
        {
            this.grid = FindObjectOfType<GridBehaviour>();
            this.goapRunner = FindObjectOfType<GoapRunnerBehaviour>();
            this.turns = this.GetComponent<TurnBehaviour>();
        }

        private void Start()
        {
            this.SpawnAgent("TurnBased", Color.red);
            this.SpawnAgent("TurnBased", Color.blue);
        }

        private void SpawnAgent(string setId, Color color)
        {
            var agent = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<AgentBehaviour>();
            
            agent.GoapSet = this.goapRunner.GetGoapSet(setId);
            agent.gameObject.SetActive(true);
            
            agent.gameObject.transform.name = $"agent {agent.GetInstanceID()}";

            var spriteRenderer = agent.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = color;

            this.turns.Register(agent);
        }
        
        private Vector3 GetRandomPosition()
        {
            var tile = this.grid.GetWalkableTiles().Random();
            
            return new Vector3(tile.X, 0.5f, tile.Y);
        }
    }
}