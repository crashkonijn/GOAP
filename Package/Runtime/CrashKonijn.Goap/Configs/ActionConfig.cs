using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.Serializables;
using UnityEngine;

namespace CrashKonijn.Goap.Configs
{
    public interface IActionConfig : IClassConfig
    {
        int BaseCost { get; }
        ITargetKey Target { get; }
        float InRange { get; }
        
        ICondition[] Conditions { get; }
        IEffect[] Effects { get; }
    }
}