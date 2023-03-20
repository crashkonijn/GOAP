using System;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.Complex.Behaviours
{
    public class AgentSpawnBehaviour : MonoBehaviour
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);
        
        private IGoapRunner goapRunner;
        
        [SerializeField]
        private GameObject agentPrefab;

        private void Awake()
        {
            this.goapRunner = FindObjectOfType<GoapRunnerBehaviour>();
            this.agentPrefab.SetActive(false);
        }

        private void Start()
        {
            var set = this.goapRunner.Sets.First();
            
            this.SpawnAgent(set);
        }

        private void SpawnAgent(IGoapSet goapSet)
        {
            var agent = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            agent.gameObject.SetActive(true);
        }
        
        private Vector3 GetRandomPosition()
        {
            var randomX = Random.Range(-Bounds.x, Bounds.x);
            var randomY = Random.Range(-Bounds.y, Bounds.y);
            
            return new Vector3(randomX, 0f, randomY);
        }
    }
}