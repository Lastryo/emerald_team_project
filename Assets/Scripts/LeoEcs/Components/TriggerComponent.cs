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

        public bool IsInteractive
        {
            get => UnityTrigger.GetComponent<Collider>().isTrigger;
            set
            {
                UnityTrigger.GetComponent<Collider>().isTrigger = value;

                /* if (!value)
                     if (OutActions != null && OutActions.Count > 0)
                         foreach (var action in OutActions)
                             action?.Invoke();*/
            }
        }

        public List<Action> InActions { get; set; }
        public List<Action> OutActions { get; set; }

        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            ref var trigger = ref entity.Get<TriggerComponent>();
            InActions = new List<Action>();
            OutActions = new List<Action>();
            component = trigger = this;

            UnityTrigger.Entity = entity;
        }
    }
}