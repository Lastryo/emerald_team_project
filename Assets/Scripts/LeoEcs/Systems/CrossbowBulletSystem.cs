using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class CrossbowBulletSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<ProjectilePointComponent> pointFilter = null;
        readonly EcsFilter<BulletComponent>.Exclude<BulletShootingComponent> isReadyToShootFilter = null;
        readonly EcsFilter<TopDownControllerComponent, FollowCameraComponent>.Exclude<DeathComponent> _characterFilter;
        readonly EcsFilter<ShootEvent> shootEvent = null;


        public void Run()
        {
            AddBulletToWeapon();
            Shoot();
        }

        private void AddBulletToWeapon()
        {
            if (pointFilter.IsEmpty()) return;

            foreach (var item in pointFilter)
            {
                ref var entity = ref pointFilter.GetEntity(item);
                if (entity.Has<BusyProjectilePointComponent>())
                {
                    ref var busyProjectioleComponent = ref entity.Get<BusyProjectilePointComponent>();
                    ref var pointComponent = ref pointFilter.Get1(item);
                    busyProjectioleComponent.Bullet.transform.position = pointComponent.point.position;
                    busyProjectioleComponent.Bullet.transform.rotation = pointComponent.point.rotation;
                }
                else
                {
                    ref var pointComponent = ref pointFilter.Get1(item);
                    var bullet = GameObject.Instantiate(pointFilter.Get1(item).Bullet, pointComponent.point.position, Quaternion.identity).transform;
                    entity.Get<BusyProjectilePointComponent>().Bullet = bullet;
                }

            }
        }

        private void Shoot()
        {
            if (isReadyToShootFilter.IsEmpty()) return;
            if (shootEvent.IsEmpty()) return;
            if (pointFilter.IsEmpty()) return;

            foreach (var item in pointFilter)
            {
                ref var pointEntity = ref pointFilter.GetEntity(default);
                if (pointEntity.Has<BusyProjectilePointComponent>())
                {
                    Debug.Log("Выстрел");
                    ref var bulletEntity = ref isReadyToShootFilter.GetEntity(default);
                    ref var bulletComponent = ref isReadyToShootFilter.Get1(default);
                    var ea = bulletComponent.transform.transform.rotation.eulerAngles;
                    bulletComponent.transform.transform.rotation = Quaternion.Euler(-90f, ea.y, ea.z);
                    bulletEntity.Get<BulletShootingComponent>();
                    pointEntity.Del<BusyProjectilePointComponent>();
                }
            }
        }
    }

}