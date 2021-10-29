using System;
using System.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class SpawnSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;

        readonly EcsFilter<SpawnPointComponent>.Exclude<StopSpawnMarkerComponent> spawnFilter = null;

        public void Run()
        {
            if (spawnFilter.IsEmpty()) return;

            foreach (var item in spawnFilter)
            {
                ref var spawnEntity = ref spawnFilter.GetEntity(item);
                ref var spawnComponent = ref spawnFilter.Get1(item);
                Spawn(spawnComponent.prefab, spawnComponent.point.transform.position);

                spawnEntity.Get<StopSpawnMarkerComponent>();
            }
        }


        public async void Spawn(GameObject prefab, Vector3 position)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            GameObject.Instantiate(prefab, position, Quaternion.identity);
        }
    }

}