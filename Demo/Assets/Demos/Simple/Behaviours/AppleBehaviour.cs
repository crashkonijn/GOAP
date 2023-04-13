using UnityEngine;

namespace Demos.Simple.Behaviours
{
    public class AppleBehaviour : MonoBehaviour
    {
        public float nutritionValue = 50f;
        public bool IsPickedUp { get; private set; }
        
        private AppleCollection appleCollection;
        
        private void Awake()
        {
            this.nutritionValue = Random.Range(80f, 150f);
            this.appleCollection = FindObjectOfType<AppleCollection>();
        }

        private void OnEnable()
        {
            this.appleCollection.Add(this);
        }
        
        private void OnDisable()
        {
            this.appleCollection.Remove(this);
        }

        public void PickUp()
        {
            this.IsPickedUp = true;
            this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        
        public void Drop()
        {
            this.IsPickedUp = false;
            this.appleCollection.Remove(this);
        }
    }
}