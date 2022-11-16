using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoalConfig")]
    public class GoalConfig : ScriptableObject
    {
        [HideInInspector]
        public string goalClass;

        public int baseCost = 1;

        public List<Condition> conditions;
    }
}