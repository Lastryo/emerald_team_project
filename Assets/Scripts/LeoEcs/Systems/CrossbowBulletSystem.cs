using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class CrossbowBulletSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<ProjectilePointComponent>.Exclude<BusyProjectilePointComponent> freePointFilter = null;
        readonly EcsFilter<BulletComponent>.Exclude<BulletShootingComponent> isReadyToShootFilter = null;
        readonly EcsFilter<ShootEvent> shootEvent = null;


        public void Run()
        {
            AddBulletToWeapon();
            Shoot();
        }

        private void AddBulletToWeapon()
        {
            if (freePointFilter.IsEmpty()) return;

            foreach (var item in freePointFilter)
            {
                ref var pointComponent = ref freePointFilter.Get1(item);
                ref var entity = ref freePointFilter.GetEntity(item);
                GameObject.Instantiate(freePointFilter.Get1(item).Bullet, pointComponent.point);
                entity.Get<BusyProjectilePointComponent>();
            }
        }

        private void Shoot()
        {
            if (isReadyToShootFilter.IsEmpty()) return;
            if (shootEvent.IsEmpty()) return;

            Debug.Log("Выстрел");

            // накинуть BulletShootingComponent  
            // снять BusyProjectilePointComponent

        }
    }

}