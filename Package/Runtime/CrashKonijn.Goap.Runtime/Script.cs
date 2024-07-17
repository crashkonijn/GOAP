using System;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [Serializable]
    public class Script
    {
        [field: SerializeField]
        public string Name { get; set; }
        
        [SerializeField]
        private string fullName;
        private Type type;
        public Type Type
        {
            get
            {
                if (type == null)
                    type = Type.GetType(fullName);
                
                return type;
            }
            set
            {
                type = value;
                fullName = value.AssemblyQualifiedName;
            }
        }
        [field: SerializeField]
        public string Path { get; set; }
        [field: SerializeField]
        public string Id { get; set; }
    }
}