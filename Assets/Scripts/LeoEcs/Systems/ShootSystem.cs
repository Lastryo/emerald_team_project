using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class ShootSystem : IEcsRunSystem
    {

        readonly EcsWorld _world = null;
        readonly EcsFilter<BulletComponent, BulletShootingComponent> shootingBulletFilter;
        readonly EcsFilter<TopDownControllerComponent, FollowCameraComponent>.Exclude<DeathComponent> _characterFilter;

        public void Run()
        {
            if (shootingBulletFilter.IsEmpty()) return;
            foreach (var item in shootingBulletFilter)
            {
                ref var bulletComponent = ref shootingBulletFilter.Get1(item);

                bulletComponent.transform.position += bulletComponent.direction * bulletComponent.speed * Time.deltaTime;

                if (bulletComponent.coveredDistance >= _characterFilter.Get1(default).Range)
                {
                    GameObject.Destroy(bulletComponent.transform.gameObject);
                    shootingBulletFilter.GetEntity(item).Destroy();

                }
            }
        }
    }

}