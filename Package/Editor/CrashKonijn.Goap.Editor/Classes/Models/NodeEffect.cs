using System;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Editor.Classes.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Guid[] Connections { get; set; } = {};
    }
}