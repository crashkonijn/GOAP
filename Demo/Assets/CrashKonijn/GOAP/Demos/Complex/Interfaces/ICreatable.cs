using System;
using System.Collections.Generic;

namespace Demos.Complex.Interfaces
{
    public interface ICreatable : IHoldable
    {
        public int BuildPoints { get; set; }
    }
}