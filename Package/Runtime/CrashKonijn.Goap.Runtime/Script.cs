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
                if (this.type == null) this.type = Type.GetType(this.fullName);

                return this.type;
            }
            set
            {
                this.type = value;
                this.fullName = value.AssemblyQualifiedName;
            }
        }

        [field: SerializeField]
        public string Path { get; set; }

        [field: SerializeField]
        public string Id { get; set; }
    }
}
