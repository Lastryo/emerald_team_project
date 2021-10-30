using System;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client
{
    struct SpawnEnemyComponent : IComponent
    {
        public Transform Point;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public EnemySpawnData SpawnData;
        public float SpawnInterval;
        public float currentTime;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<SpawnEnemyComponent>() = this;
        }
    }
}