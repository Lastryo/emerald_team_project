using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class ShootSystem : IEcsRunSystem
    {

        readonly EcsWorld _world = null;
        readonly EcsFilter<BulletComponent, BulletShootingComponent> shootingBulletFilter;

        public void Run()
        {
            if (shootingBulletFilter.IsEmpty()) return;

            foreach (var item in shootingBulletFilter)
            {
                ref var bulletComponent = ref shootingBulletFilter.Get1(item);
                bulletComponent.transform.Translate(-bulletComponent.transform.forward * bulletComponent.speed * Time.deltaTime);
            }
        }
    }

}