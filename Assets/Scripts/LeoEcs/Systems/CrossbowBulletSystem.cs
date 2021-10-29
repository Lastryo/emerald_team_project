using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class CrossbowBulletSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<CrossbowComponent> pointFilter = null;
        readonly EcsFilter<BulletComponent>.Exclude<BulletShootingComponent> isReadyToShootFilter = null;
        readonly EcsFilter<TopDownControllerComponent, FollowCameraComponent>.Exclude<DeathComponent> _characterFilter;
        readonly EcsFilter<ShootEvent> shootEvent = null;
        readonly EcsFilter<ShootInputEvent> shootInputEvent = null;

        private bool isPuched = false;

        public void Run()
        {
            AddBulletToWeapon();
            Shoot();
            ShootProcess();
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
            if (shootInputEvent.IsEmpty()) return;
            if (pointFilter.IsEmpty()) return;

            foreach (var item in pointFilter)
            {
                ref var pointEntity = ref pointFilter.GetEntity(default);
                if (!isPuched)
                {
                    Debug.Log("Выстрел");
                    ref var bulletEntity = ref isReadyToShootFilter.GetEntity(default);
                    ref var bulletComponent = ref isReadyToShootFilter.Get1(default);
                    ref var characterEntity = ref _characterFilter.GetEntity(default);

                    ref var animationComponent = ref characterEntity.Get<AnimationComponent>();
                    var ea = bulletComponent.transform.transform.rotation.eulerAngles;
                    bulletComponent.transform.transform.rotation = Quaternion.Euler(-90f, ea.y, ea.z);

                    animationComponent.animator.SetTrigger("Shoot");
                    pointEntity.Get<CrossbowComponent>().animation.SetTrigger("Shoot");
                    isPuched = true;
                }
            }
        }

        private void ShootProcess()
        {
            if (shootEvent.IsEmpty()) return;
            if (pointFilter.IsEmpty()) return;
            if (isReadyToShootFilter.IsEmpty()) return;
            if (!isPuched) return;
            ref var bulletEntity = ref isReadyToShootFilter.GetEntity(default);
            ref var bulletComponent = ref isReadyToShootFilter.Get1(default);
            ref var pointEntity = ref pointFilter.GetEntity(default);

            foreach (var item in pointFilter)
            {
                bulletEntity.Get<BulletShootingComponent>();
                pointEntity.Del<BusyProjectilePointComponent>();
                isPuched = false;
            }
        }
    }

}