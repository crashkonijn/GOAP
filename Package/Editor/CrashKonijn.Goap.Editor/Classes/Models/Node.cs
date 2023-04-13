using System;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Editor.Classes.Models
{
    public class Node
    {
        public Guid Guid => this.Action.Guid;
        
        public IAction Action { get; set; }

        public NodeEffect[] Effects { get; set; } = {};
        public NodeCondition[] Conditions { get; set; } = {};
    }
}