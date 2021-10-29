using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class AnimationEventProvider : MonoBehaviour, IHaveEntity
    {
        public EcsEntity Entity { get; set; }

        private void Shoot()
        {
            EcsStartup.World.NewEntity().Get<ShootEvent>();
        }

        private void Attack()
        {
            EcsStartup.World.NewEntity().Get<ResetEnemyAttackEvent>().entity = Entity;
        }
    }
}