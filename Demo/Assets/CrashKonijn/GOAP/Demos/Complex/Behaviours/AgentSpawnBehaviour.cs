using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Classes;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class AgentSpawnBehaviour : MonoBehaviour
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);
        
        private IGoap goap;
        
        [SerializeField]
        private GameObject agentPrefab;

        public Color cleanerColor;
        public Color smithColor;
        public Color minerColor;
        public Color woodCutterColor;

        private void Awake()
        {
            this.goap = FindObjectOfType<GoapBehaviour>();
            this.agentPrefab.SetActive(false);
        }

        private void Start()
        {
            this.SpawnAgent(SetIds.Cleaner, ComplexAgentBrain.AgentType.Cleaner, this.cleanerColor);
            this.SpawnAgent(SetIds.Cleaner, ComplexAgentBrain.AgentType.Cleaner, this.cleanerColor);
            this.SpawnAgent(SetIds.Cleaner, ComplexAgentBrain.AgentType.Cleaner, this.cleanerColor);
            
            this.SpawnAgent(SetIds.Smith, ComplexAgentBrain.AgentType.Smith, this.smithColor);
            this.SpawnAgent(SetIds.Miner, ComplexAgentBrain.AgentType.Miner, this.minerColor);
            this.SpawnAgent(SetIds.WoodCutter, ComplexAgentBrain.AgentType.WoodCutter, this.woodCutterColor);
        }

        private void SpawnAgent(string setId, ComplexAgentBrain.AgentType agentType, Color color)
        {
            var agent = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<GoapActionProvider>();
            
            agent.AgentType = this.goap.GetAgentType(setId);
            agent.gameObject.SetActive(true);
            
            agent.gameObject.transform.name = $"{agentType} {agent.GetInstanceID()}";

            var brain = agent.GetComponent<ComplexAgentBrain>();
            brain.agentType = agentType;

            var spriteRenderer = agent.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = color;
        }
        
        private Vector3 GetRandomPosition()
        {
            var randomX = Random.Range(-Bounds.x, Bounds.x);
            var randomY = Random.Range(-Bounds.y, Bounds.y);
            
            return new Vector3(randomX, 0f, randomY);
        }
    }
}