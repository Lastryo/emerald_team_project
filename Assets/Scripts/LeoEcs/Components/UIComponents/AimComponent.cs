using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct AimComponent : IComponent
    {
        public RectTransform rect;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за UI прицела";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<AimComponent>() = this;
        }
    }
}