using Leopotam.Ecs;
using Unity.VisualScripting;
using UnityEngine;

namespace Client
{
    sealed class SimpleAiSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<TopDownAiComponent>.Exclude<DeathComponent> enemyFilter;
        readonly EcsFilter<TopDownControllerComponent>.Exclude<DeathComponent> characterFilter;
        readonly EcsFilter<ResetEnemyAttackEvent> resetEventFilter;



        public void Run()
        {
            if (enemyFilter.IsEmpty()) return;
            if (characterFilter.IsEmpty()) return;

            foreach (var item in enemyFilter)
            {
                ref var enemyController = ref enemyFilter.Get1(item);
                ref var characterController = ref characterFilter.Get1(default);
                ref var enemyEntity = ref enemyFilter.GetEntity(item);
                if (Vector3.Distance(characterController.Transform.position, enemyController.transform.position) > enemyController.attackRange)
                {
                    RunToTarget(ref enemyController, ref characterController);
                }
                else
                {
                    Attack(ref enemyEntity);
                }
            }

            ResetAttack();
        }

        private void ResetAttack()
        {
            if (enemyFilter.IsEmpty()) return;
            if (resetEventFilter.IsEmpty()) return;

            foreach (var evt in resetEventFilter)
            {
                foreach (var item in enemyFilter)
                {
                    ref var enemy = ref enemyFilter.GetEntity(item);
                    ref var attackEntity = ref resetEventFilter.Get1(item);
                    if (enemy.AreEquals(attackEntity.entity))
                    {
                        enemy.Del<InAttackMarkerComponent>();
                        return;
                    }
                }
            }
        }

        private void Attack(ref EcsEntity entity)
        {
            ref var ai = ref entity.Get<TopDownAiComponent>();
            if (ai.animation.GetBool("Move"))
                ai.animation.SetBool("Move", false);
            if (entity.Has<InAttackMarkerComponent>()) return;

            Debug.Log("Надо атаковать");
            ai.animation.SetInteger("ActionAttack", Random.Range(0, 4));
            ai.animation.SetTrigger("Attack");
            entity.Get<InAttackMarkerComponent>();
        }

        private void RunToTarget(ref TopDownAiComponent aiComponent, ref TopDownControllerComponent characterComponent)
        {
            aiComponent.agent.SetDestination(characterComponent.Transform.position);
            if (aiComponent.animation.GetBool("Move")) return;
            aiComponent.animation.SetBool("Move", true);
        }
    }

}