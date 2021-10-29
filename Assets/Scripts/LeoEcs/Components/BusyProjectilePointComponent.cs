using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct BusyProjectilePointComponent : IComponent
    {
        public Transform Bullet;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������ ������� ������� ����� ������� �������";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<BusyProjectilePointComponent>() = this;
        }
    }
}