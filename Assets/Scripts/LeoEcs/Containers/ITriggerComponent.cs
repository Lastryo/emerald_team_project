using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public interface ITriggerComponent : IComponent
    {
        LayerMask Mask { get; set; }
        List<Action> InActions { get; set; }
        List<Action> OutActions { get; set; }
    }
}
