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
        public string Doc => "��������� ������� ������������� �� ��������� ���� �� ��� ������ ������";
#endif

        [HideInInspector]
        public Camera camera;
        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<FollowCameraComponent>() = this;
        }
    }
}