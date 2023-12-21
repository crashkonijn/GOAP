using System;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Classes.Sources
{
    public class BoxSource : MonoBehaviour
    {
        public Type ItemType { get; set; }
        
        public void Place(IHoldable item)
        {
            item.Drop(true);
            item.gameObject.transform.position = this.transform.position + new Vector3(0f, 0.1f, 0f);
        }
    }
}