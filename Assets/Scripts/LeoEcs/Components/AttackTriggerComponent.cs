using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct AttackTriggerComponent : IComponent
    {
        public LayerMask Mask;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<AttackTriggerComponent>() = this;
        }
    }
}