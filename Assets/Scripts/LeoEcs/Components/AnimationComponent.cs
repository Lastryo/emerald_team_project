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
        public string Doc => "��������� ������� �������� ��...";
#endif
        public Animator animator;
        public void SetOwner(in EcsEntity entity)
        {

        }

        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<AnimationComponent>() = this;
        }
    }
}