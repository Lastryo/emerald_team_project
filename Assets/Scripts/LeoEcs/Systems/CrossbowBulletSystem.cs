using System.Linq;
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
        readonly EcsFilter<ChangeBulletEvent> changeBulletEvent = null;
        readonly EcsFilter<ShootEvent> shootEvent = null;
        readonly EcsFilter<ShootInputEvent> shootInputEvent = null;

        private bool isPuched = false;

        public void Run()
        {
            ChangeBullet();
            AddBulletToWeapon();
            Shoot();
            ShootProcess();
        }

        private void ChangeBullet()
        {
            if (changeBulletEvent.IsEmpty()) return;
            if (pointFilter.IsEmpty()) return;

            foreach (var item in changeBulletEvent)
            {
                ref var evt = ref changeBulletEvent.Get1(item);
                ref var entity = ref pointFilter.GetEntity(default);
                pointFilter.Get1(default).currentType = evt.Type;
                GameObject.Destroy(isReadyToShootFilter.Get1(default).transform.gameObject);
                isReadyToShootFilter.GetEntity(default).Destroy();
                CreateBullet(default);
            }
        }

        private void CreateBullet(int index)
        {
            ref var pointComponent = ref pointFilter.Get1(index);
            ref var entity = ref pointFilter.GetEntity(index);

            var currentType = pointComponent.currentType;
            var neededData = pointComponent.data.FirstOrDefault(x => x.type == currentType);
            var bullet = GameObject.Instantiate(neededData.Bullet, pointComponent.point.position, Quaternion.identity).transform;
            entity.Get<BusyProjectilePointComponent>().Bullet = bullet;
            bullet.GetComponent<Collider>().enabled = false;
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
                    CreateBullet(item);

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
                    ref var topDownComponent = ref _characterFilter.Get1(default);

                    ref var animationComponent = ref characterEntity.Get<AnimationComponent>();
                    ref var projectilePointer = ref characterEntity.Get<ProjectilePointerComponent>();
                    var heading = topDownComponent.FinalLookPosition - topDownComponent.Transform.position;
                    var dir = heading.normalized;
                    bulletComponent.direction = dir;
                    bulletComponent.transform.rotation = projectilePointer.ProjectilePointer.rotation;
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
                bulletComponent.transform.GetComponent<Collider>().enabled = true;

                bulletEntity.Get<BulletShootingComponent>();
                pointEntity.Del<BusyProjectilePointComponent>();
                isPuched = false;
            }
        }
    }

}