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
                triggerComponent.InActions.Add((e) =>
                {
                    _world.NewEntity().Get<DamageEvent>().damagedEntity = e;
                });

                entity.Get<DamageInitedMarkerComponent>();
            }
        }

        private void SetDamage()
        {
            if (damageEvent.IsEmpty()) return;

            foreach (var item in damageEvent)
            {
                var entity = damageEvent.Get1(item).damagedEntity;
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