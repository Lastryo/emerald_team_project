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
        public string Doc => "Компонент который отвечает за...";
#endif

        public Transform point;
        public GameObject prefab;
        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<SpawnPointComponent>() = this;
        }
    }
}