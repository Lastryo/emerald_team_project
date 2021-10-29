using Cinemachine;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Client
{
    struct CameraComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который содержит данные о камере";
#endif
        public Camera camera;
        public CinemachineVirtualCamera virtualCamera;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<CameraComponent>() = this;
        }
    }
}