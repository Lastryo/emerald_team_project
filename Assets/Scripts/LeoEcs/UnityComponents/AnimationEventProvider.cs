using System;
using System.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class AnimationEventProvider : MonoBehaviour, IHaveEntity
    {
        public EcsEntity Entity { get; set; }
        public float attackDelay = 2;
        public Collider AttackCollider;

        private void Shoot()
        {
            EcsStartup.World.NewEntity().Get<ShootEvent>();
        }

        private async void Attack()
        {
            await Task.Delay(TimeSpan.FromSeconds(attackDelay));
            EcsStartup.World.NewEntity().Get<ResetEnemyAttackEvent>().entity = Entity;
        }

        private void StartAttack()
        {
            AttackCollider.enabled = true;
        }

        private void EndAttack()
        {
            AttackCollider.enabled = false;
        }
    }
}