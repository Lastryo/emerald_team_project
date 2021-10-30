using System;
using System.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client
{
    sealed class SpawnSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;

        readonly EcsFilter<SpawnPointComponent>.Exclude<StopSpawnMarkerComponent> spawnFilter = null;
        readonly EcsFilter<SpawnEnemyComponent>.Exclude<StopSpawnMarkerComponent> enemySpawnFilter = null;

        public void Run()
        {
            SpawnPlayer();
            SpawnEnemies();
        }


        public void SpawnPlayer()
        {
            if (spawnFilter.IsEmpty()) return;
            Cursor.visible = false;

            foreach (var item in spawnFilter)
            {
                ref var spawnEntity = ref spawnFilter.GetEntity(item);
                ref var spawnComponent = ref spawnFilter.Get1(item);
                Spawn(spawnComponent.prefab, spawnComponent.point.transform.position);

                spawnEntity.Get<StopSpawnMarkerComponent>();
            }
        }

        public void SpawnEnemies()
        {
            if (enemySpawnFilter.IsEmpty()) return;

            foreach (var item in enemySpawnFilter)
            {
                ref var spawnEntity = ref enemySpawnFilter.GetEntity(item);
                ref var spawnComponent = ref enemySpawnFilter.Get1(item);
                if (spawnComponent.currentTime >= spawnComponent.SpawnInterval)
                {
                    ReactiveSpawn(spawnComponent.SpawnData.reference, spawnComponent.Point.transform.position);
                    spawnComponent.currentTime = 0;
                }
                else
                {
                    spawnComponent.currentTime += Time.deltaTime;
                }
            }
        }


        public async void Spawn(GameObject prefab, Vector3 position)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            GameObject.Instantiate(prefab, position, Quaternion.identity);
        }

        public void ReactiveSpawn(AssetReference reference, Vector3 position)
        {
            reference.InstantiateAsync(position, Quaternion.identity);
        }
    }

}