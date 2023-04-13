using UnityEngine;

namespace Demos.Simple.Behaviours
{
    public class TreeBehaviour : MonoBehaviour
    {
        public GameObject applePrefab;

        public AppleBehaviour DropApple()
        {
            var random = Random.insideUnitCircle * 3f;
            var position = new Vector3(random.x, 0f, random.y);

            return Instantiate(this.applePrefab, this.transform.position + position, Quaternion.identity).GetComponent<AppleBehaviour>();
        }
    }
}