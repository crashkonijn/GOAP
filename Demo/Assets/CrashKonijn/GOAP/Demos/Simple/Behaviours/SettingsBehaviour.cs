using CrashKonijn.Goap.Behaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Demos.Simple.Behaviours
{
    public class SettingsBehaviour : MonoBehaviour
    {
        private static readonly Vector2 Bounds = new Vector2(15, 8);
        
        public GameObject applePrefab;
        public GameObject agentPrefab;
        public GoapSetBehaviour goapSet;
        
        public TextMeshProUGUI appleCountText;
        public TextMeshProUGUI agentCountText;
        public TextMeshProUGUI fpsText;
        public Toggle debugToggle;

        private bool debug = true;
        private GoapRunnerBehaviour goapRunner;

        private int frameCount;
        private float fps;
        private float fpsTimer;
        
        private AppleCollection apples;

        private void Awake()
        {
            this.agentPrefab.SetActive(false);
            this.goapRunner = FindObjectOfType<GoapRunnerBehaviour>();
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
            
            this.fpsText.text = $"FPS: {this.fps}\nResolve count: {this.goapRunner.RunCount}\nRunTime: {this.goapRunner.RunTime} (ms)\nCompleteTime: {this.goapRunner.CompleteTime} (ms)";
        }

        private void FixedUpdate()
        {
            this.appleCountText.text = $"+ Apple ({this.apples.Get().Length})";
            this.agentCountText.text = $"+ Agent ({this.goapRunner.Agents.Length})";
        }

        public void SetDebug(bool value)
        {
            this.debug = value;
            
            if (this.debugToggle.isOn != value)
                this.debugToggle.isOn = value;
            
            foreach (var textBehaviour in FindObjectsOfType<TextBehaviour>())
            {
                this.SetDebug(textBehaviour, value);
            }
        }

        private void SetDebug(TextBehaviour textBehaviour, bool value)
        {
            textBehaviour.enabled = value;
            textBehaviour.GetComponentInChildren<Canvas>(true).gameObject.SetActive(value);
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
            
            var agentCount = this.goapRunner.Agents.Length;
            var count = agentCount < 50 ? 50 - agentCount : 50;
            
            for (var i = 0; i < count; i++)
            {
                var agent = Instantiate(this.agentPrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<AgentBehaviour>();
                agent.GoapSet = this.goapSet.GoapSet;
            
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