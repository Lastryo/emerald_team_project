using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct CrossbowComponent : IComponent
    {
        public Transform point;
        public GameObject Bullet;
        public Animator animation;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за хранение пули";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<CrossbowComponent>() = this;
        }
    }
}