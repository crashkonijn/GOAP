using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Editor
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Guid[] Connections { get; set; } = {};
    }
}