using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalWorldSensorBase : IGlobalWorldSensor
    {
        public IWorldKey Key => this.Config.Key;

        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => this.Config = config;

        public abstract void Created();
        public Type[] GetKeys() => new[] { this.Key.GetType() };
        
        public void Sense(IWorldData data)
        {
            data.SetState(this.Key, this.Sense());
        }
        
        public abstract SenseValue Sense();
    }
}