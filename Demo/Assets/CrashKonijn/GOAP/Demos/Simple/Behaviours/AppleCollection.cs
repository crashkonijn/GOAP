using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class AppleCollection : MonoBehaviour
    {
        private readonly List<AppleBehaviour> apples = new();
        
        public void Add(AppleBehaviour apple)
        {
            this.apples.Add(apple);
        }
        
        public void Remove(AppleBehaviour apple)
        {
            this.apples.Remove(apple);
        }

        public List<AppleBehaviour> Get()
        {
            return this.apples;
        }

        public bool Any()
        {
            return this.apples.Any();
        }
    }
}