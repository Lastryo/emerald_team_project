using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public interface ITriggerComponent : IComponent
    {
        List<Action<EcsEntity, EcsEntity>> InActions { get; set; }
        List<Action<EcsEntity, EcsEntity>> OutActions { get; set; }
    }
}
