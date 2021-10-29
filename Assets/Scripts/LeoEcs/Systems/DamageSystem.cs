using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class DamageSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<TopDownAiComponent, TriggerComponent>.Exclude<DamageInitedMarkerComponent> notInitAiFilter;
        readonly EcsFilter<DamageEvent> damageEvent;
        public void Init()
        {
            if (notInitAiFilter.IsEmpty()) return;

            foreach (var item in notInitAiFilter)
            {
                ref var triggerComponent = ref notInitAiFilter.Get2(item);
                ref var entity = ref notInitAiFilter.GetEntity(item);
                triggerComponent.InActions.Add((e, b) =>
                {
                    ref var entity = ref _world.NewEntity().Get<DamageEvent>();
                    entity.damagedEntity = e;
                    entity.damagerEntity = b;
                });

                entity.Get<DamageInitedMarkerComponent>();
            }
        }

        private void SetDamage()
        {
            if (damageEvent.IsEmpty()) return;

            foreach (var item in damageEvent)
            {
                ref var evt = ref damageEvent.Get1(item);
                var entity = evt.damagedEntity;
                if (evt.damagerEntity.IsNull())
                    continue;
                if (evt.damagedEntity.Get<EnemyTypeComponent>().Type !=
            evt.damagerEntity.Get<EnemyTypeComponent>().Type) continue;

                ref var changeHP = ref entity.Get<ChangeHPComponent>();
                changeHP.Type = ChangeHPComponent.ChangeHealthType.Damage;
                changeHP.Value = 20;
                Debug.Log("Получил урон");
            }
        }

        public void Run()
        {
            Init();
            SetDamage();
        }
    }

}