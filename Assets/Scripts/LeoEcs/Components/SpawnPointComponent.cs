using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client
{
    struct SpawnPointComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������� �������� ��...";
#endif

        public Transform point;
        public GameObject prefab;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<SpawnPointComponent>() = this;
        }
    }
}