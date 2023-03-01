using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demos.Behaviours
{
    public class InventoryBehaviour : MonoBehaviour
    {
        public List<AppleBehaviour> Apples = new ();

        public void Put(AppleBehaviour apple)
        {
            apple.GetComponent<Renderer>().enabled = false;
            this.Apples.Add(apple);
        }

        public AppleBehaviour Get()
        {
            var apple = this.Apples.FirstOrDefault();

            if (apple == null)
                return null;

            this.Apples.Remove(apple);
            
            return apple;
        }
    }
}