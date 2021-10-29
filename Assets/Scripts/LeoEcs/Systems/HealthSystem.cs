using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class HealthSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        public EcsFilter<HPComponent, ChangeHPComponent> changeHPFilter;

        public void Run()
        {
            ChangeHP();
        }

        private void ChangeHP()
        {
            if (changeHPFilter.IsEmpty()) return;

            foreach (var item in changeHPFilter)
            {
                ref var hpComponent = ref changeHPFilter.Get1(item);
                ref var changeHPComponent = ref changeHPFilter.Get2(item);
                ref var entity = ref changeHPFilter.GetEntity(item);

                switch (changeHPComponent.Type)
                {
                    case ChangeHPComponent.ChangeHealthType.Damage:
                        if (hpComponent.HP > changeHPComponent.Value)
                        {
                            hpComponent.HP -= changeHPComponent.Value;
                            //анимация получения урона
                            Debug.Log($"Получил урон {changeHPComponent.Value}");
                        }
                        else
                        {
                            hpComponent.HP = 0;
                            entity.Get<DeathComponent>();
                            entity.Get<RagdollComponent>().Ragdoll.isActive = false;
                            Debug.Log($"Умер");
                            // анимация смерти
                        }
                        break;
                    case ChangeHPComponent.ChangeHealthType.Heal:
                        var prevHP = hpComponent.HP;
                        if (changeHPComponent.Value + hpComponent.HP >= hpComponent.MaxHP)
                            hpComponent.HP = hpComponent.MaxHP;
                        else hpComponent.HP += changeHPComponent.Value;

                        if (prevHP > 0)
                        {
                            Debug.Log($"Полечился на {changeHPComponent.Value}");
                            //анимация получения здоровья
                        }
                        else
                        {
                            Debug.Log($"Воскрес");
                            //анимация воскрешения
                        }
                        break;
                }

                entity.Del<ChangeHPComponent>();
            }
        }
    }
}