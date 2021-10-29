using System;
using System.Collections.Generic;
using Leopotam.Ecs;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
#endif
using UnityEngine;

namespace Client
{
    public struct TriggerComponent : ITriggerComponent
    {
        [field: SerializeField]
        public LayerMask Mask { get; set; }
        public UnityTriggerComponent UnityTrigger;

        public List<Action<EcsEntity, EcsEntity>> InActions { get; set; }
        public List<Action<EcsEntity, EcsEntity>> OutActions { get; set; }

        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            ref var trigger = ref entity.Get<TriggerComponent>();
            InActions = new List<Action<EcsEntity, EcsEntity>>();
            OutActions = new List<Action<EcsEntity, EcsEntity>>();
            component = trigger = this;
            UnityTrigger.Entity = entity;
        }
    }
}