using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Editor.Classes.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Guid[] Connections { get; set; } = {};
    }
}