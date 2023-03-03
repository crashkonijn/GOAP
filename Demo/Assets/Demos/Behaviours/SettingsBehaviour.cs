using System;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.Behaviours
{
    
    public class SettingsBehaviour : MonoBehaviour
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);
        
        public GameObject applePrefab;
        public GameObject agentPrefab;
        public GoapSetBehaviour goapSet;
        
        public TextMeshProUGUI appleCountText;
        public TextMeshProUGUI agentCountText;

        private bool debug = true;

        private void Awake()
        {
            this.agentPrefab.SetActive(false);
        }

        private void FixedUpdate()
        {
            this.appleCountText.text = $"+ Apple ({FindObjectsOfType<AppleBehaviour>().Count(x => x.GetComponentInChildren<SpriteRenderer>().enabled)})";
            this.agentCountText.text = $"+ Agent ({FindObjectsOfType<AgentBehaviour>().Length})";
        }

        public void SetDebug(bool value)
        {
            this.debug = value;
            
            foreach (var textBehaviour in FindObjectsOfType<TextBehaviour>())
            {
                this.SetDebug(textBehaviour, value);
            }
        }

        private void SetDebug(TextBehaviour textBehaviour, bool value)
        {
            textBehaviour.enabled = value;
            textBehaviour.GetComponentInChildren<Canvas>().enabled = value;
        }

        public void SpawnApple()
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(this.applePrefab, this.GetRandomPosition(), Quaternion.identity);
            }
        }

        public void SpawnAgent()
        {
            for (int i = 0; i < 10; i++)
            {
                var agent = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<AgentBehaviour>();
                agent.GoapSet = this.goapSet.Set;
            
                this.SetDebug(agent.GetComponentInChildren<TextBehaviour>(), this.debug);
            
                agent.gameObject.SetActive(true);
            }
        }
        
        private Vector3 GetRandomPosition()
        {
            var randomX = Random.Range(-Bounds.x, Bounds.x);
            var randomY = Random.Range(-Bounds.y, Bounds.y);
            
            return new Vector3(randomX, 0f, randomY);
        }
    }
}