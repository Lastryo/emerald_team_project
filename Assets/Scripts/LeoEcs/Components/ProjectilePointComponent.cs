using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct ProjectilePointComponent : IComponent
    {
        public Transform point;
        public GameObject Bullet;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за хранение пули";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<ProjectilePointComponent>() = this;
        }
    }
}