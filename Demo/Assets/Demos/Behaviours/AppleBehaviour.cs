﻿using UnityEngine;

namespace Demos.Behaviours
{
    public class AppleBehaviour : MonoBehaviour
    {
        public float nutritionValue = 50f;

        private void Awake()
        {
            this.nutritionValue = Random.Range(40f, 60f);
        }
    }
}