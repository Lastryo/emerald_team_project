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

                GameObject.Instantiate(spawnComponent.prefab, spawnComponent.point.transform.position, Quaternion.identity);
                spawnEntity.Get<StopSpawnMarkerComponent>();
            }
        }
    }

}