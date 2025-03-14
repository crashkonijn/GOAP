using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ComplexAgentBrain : MonoBehaviour
    {
        public AgentType agentType;
        private GoapActionProvider provider;
        private ComplexHungerBehaviour complexHunger;
        private ItemCollection itemCollection;
        private ComplexInventoryBehaviour inventory;

        private void Awake()
        {
            this.provider = this.GetComponent<GoapActionProvider>();
            this.complexHunger = this.GetComponent<ComplexHungerBehaviour>();
            this.inventory = this.GetComponent<ComplexInventoryBehaviour>();
            this.itemCollection = Compatibility.FindObjectOfType<ItemCollection>();
        }

        private void OnEnable()
        {
            this.provider.Events.OnActionEnd += this.OnActionEnd;
            this.provider.Events.OnNoActionFound += this.OnNoActionFound;
            this.provider.Events.OnGoalCompleted += this.OnGoalCompleted;
        }

        private void OnDisable()
        {
            this.provider.Events.OnActionEnd -= this.OnActionEnd;
            this.provider.Events.OnNoActionFound -= this.OnNoActionFound;
            this.provider.Events.OnGoalCompleted -= this.OnGoalCompleted;
        }

        private void Start()
        {
            this.provider.RequestGoal<WanderGoal>(true);
        }
        
        private void OnNoActionFound(IGoalRequest request)
        {
            this.provider.RequestGoal<WanderGoal>(true);
        }

        private void OnGoalCompleted(IGoal goal)
        {
            this.provider.RequestGoal<WanderGoal>(true);
        }

        private void OnActionEnd(IAction action)
        {
            this.UpdateHunger();
            
            if (this.provider.GoalRequest.Goals.OfType<FixHungerGoal>().Any())
                return;
            
            this.DetermineGoal();
        }

        private void UpdateHunger()
        {
            if (this.complexHunger.hunger > 80)
            {
                this.provider.RequestGoal<FixHungerGoal>(true);
                return;
            }

            if (this.provider.CurrentPlan.Goal is not FixHungerGoal)
                return;
            
            if (this.complexHunger.hunger < 20)
                this.DetermineGoal();
        }

        private void DetermineGoal()
        {
            switch (this.agentType)
            {
                case AgentType.Miner:
                    this.DetermineMinerGoals();
                    break;
                case AgentType.WoodCutter:
                    this.DetermineWoodCutterGoals();
                    break;
                case AgentType.Smith:
                    this.DetermineSmithGoals();
                    break;
                case AgentType.Cleaner:
                    this.DetermineCleanerGoals();
                    break;
            }
        }

        private void DetermineMinerGoals()
        {
            if (this.inventory.Count<Pickaxe>() == 0 && this.itemCollection.Get<Pickaxe>().Length >= 1)
            {
                this.provider.RequestGoal<PickupItemGoal<Pickaxe>>(true);
                return;
            }
            
            if (this.itemCollection.Get<Iron>().Length <= 2)
            {
                this.provider.RequestGoal<GatherItemGoal<Iron>>(true);
                return;
            }
            
            this.provider.RequestGoal<WanderGoal>(true);
        }
        
        private void DetermineWoodCutterGoals()
        {
            if (this.inventory.Count<Axe>() == 0 && this.itemCollection.Get<Axe>().Length >= 1)
            {
                this.provider.RequestGoal<PickupItemGoal<Axe>>(true);
                return;
            }
            
            if (this.itemCollection.Get<Wood>().Length <= 2)
            {
                this.provider.RequestGoal<GatherItemGoal<Wood>>(true);
                return;
            }
            
            this.provider.RequestGoal<WanderGoal>(true);
        }
        
        private void DetermineSmithGoals()
        {
            if (this.itemCollection.Get<Axe>().Length <= 1)
            {
                this.provider.RequestGoal<CreateItemGoal<Axe>>(true);
                return;
            }
            
            if (this.itemCollection.Get<Pickaxe>().Length <= 1)
            {
                this.provider.RequestGoal<CreateItemGoal<Pickaxe>>(true);
                return;
            }

            this.provider.RequestGoal<WanderGoal>(false);
        }
        
        private void DetermineCleanerGoals()
        {
            this.provider.RequestGoal<CleanItemsGoal, FixHungerGoal, WanderGoal>(true);
        }

        public enum AgentType
        {
            Smith,
            Miner,
            WoodCutter,
            Cleaner
        }
    }
}