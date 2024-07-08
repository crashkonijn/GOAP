using CrashKonijn.Goap.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SettingsBehaviour : MonoBehaviour
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);
        
        public GameObject applePrefab;
        public GameObject agentPrefab;
        public AgentTypeBehaviour agentType;
        
        public TextMeshProUGUI appleCountText;
        public TextMeshProUGUI agentCountText;
        public TextMeshProUGUI fpsText;
        public Toggle debugToggle;

        private bool debug = true;
        private GoapBehaviour goap;

        private int frameCount;
        private float fps;
        private float fpsTimer;
        
        private AppleCollection apples;

        private void Awake()
        {
            this.agentPrefab.SetActive(false);
            this.goap = FindObjectOfType<GoapBehaviour>();
            this.apples = FindObjectOfType<AppleCollection>();
        }

        private void Update()
        {
            this.frameCount++;
            this.fpsTimer += Time.deltaTime;

            if (this.fpsTimer >= 1)
            {
                this.fps = this.frameCount;
                this.frameCount = 0;
                this.fpsTimer -= 1;
            }
            
            this.fpsText.text = $"FPS: {this.fps}\nResolve count: {this.goap.RunCount}\nRunTime: {this.goap.RunTime} (ms)\nCompleteTime: {this.goap.CompleteTime} (ms)";
        }

        private void FixedUpdate()
        {
            this.appleCountText.text = $"+ Apple ({this.apples.Get().Count})";
            this.agentCountText.text = $"+ Agent ({this.goap.Agents.Count})";
        }

        public void SetDebug(bool value)
        {
            this.debug = value;
            
            if (this.debugToggle.isOn != value)
                this.debugToggle.isOn = value;
            
            foreach (var textBehaviour in Compatibility.FindObjectsOfType<SimpleTextBehaviour>())
            {
                this.SetDebug(textBehaviour, value);
            }
        }

        private void SetDebug(SimpleTextBehaviour complexTextBehaviour, bool value)
        {
            complexTextBehaviour.enabled = value;
            complexTextBehaviour.GetComponentInChildren<Canvas>(true).gameObject.SetActive(value);
        }

        public void SpawnApple()
        {
            for (var i = 0; i < 50; i++)
            {
                Instantiate(this.applePrefab, this.GetRandomPosition(), Quaternion.identity);
            }
        }

        public void SpawnAgent()
        {
            this.SetDebug(false);
            
            var agentCount = this.goap.Agents.Count;
            var count = agentCount < 50 ? 50 - agentCount : 50;
            
            for (var i = 0; i < count; i++)
            {
                var agent = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<GoapActionProvider>();
                agent.AgentType = this.agentType.AgentType;
            
                this.SetDebug(agent.GetComponentInChildren<SimpleTextBehaviour>(), this.debug);
            
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