using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Client
{
    struct AnimationComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public Animator animator;
        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<AnimationComponent>() = this;
        }
    }
}