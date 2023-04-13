using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demos.Simple.Behaviours
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

        public AppleBehaviour[] Get()
        {
            return this.apples.ToArray();
        }

        public bool Any()
        {
            return this.apples.Any();
        }
    }
}