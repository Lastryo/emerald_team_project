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
        public string Doc => "��������� ������� �������� ������ � ������";
#endif
        public Camera camera;
        public CinemachineVirtualCamera virtualCamera;
        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<CameraComponent>() = this;
        }
    }
}