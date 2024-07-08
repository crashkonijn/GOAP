using System.Collections;
using System.Linq;
using CrashKonijn.Goap.Demos.Complex.Classes.Items;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ComplexTreeBehaviour : MonoBehaviour
    {
        private Apple apple;
        private ItemCollection itemCollection;
        private ItemFactory itemFactory;

        private void Awake()
        {
            this.itemCollection = Compatibility.FindObjectOfType<ItemCollection>();
            this.itemFactory = Compatibility.FindObjectOfType<ItemFactory>();
        }

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

            var count = this.itemCollection.All().Count(x => x is IEatable);

            if (count > 4)
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
            this.apple = this.itemFactory.Instantiate<Apple>();
            this.apple.transform.position = this.GetRandomPosition();
        }
        
        private Vector3 GetRandomPosition()
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return this.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
    }
}