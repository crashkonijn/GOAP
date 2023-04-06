using System.Collections;
using Demos.Complex.Classes.Items;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ComplexTreeBehaviour : MonoBehaviour
    {
        public GameObject applePrefab;
        private Apple apple;

        private void Start()
        {
            this.DropApple();
        }

        private void Update()
        {
            if (this.apple == null)
                return;
            
            if (!this.apple.IsHeld)
                return;
            
            this.apple = null;
            this.StartCoroutine(this.GrowApple());
        }

        private IEnumerator GrowApple()
        {
            yield return new WaitForSeconds(10f);
            this.DropApple();
        }

        private void DropApple()
        {
            this.apple = Instantiate(this.applePrefab, this.GetRandomPosition(), Quaternion.identity).GetComponent<Apple>();
        }
        
        private Vector3 GetRandomPosition()
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return this.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
    }
}