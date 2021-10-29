using Cinemachine;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Client
{
    struct FollowCameraComponent : IComponent
    {

#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который накладывается на персонажа если за ним следит камера";
#endif

        [HideInInspector]
        public Camera camera;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<FollowCameraComponent>() = this;
        }
    }
}