using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class HealthSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        public EcsFilter<HPComponent, ChangeHPComponent> changeHPFilter;
        public EcsFilter<DeathScreenComponent> deathScreenFilter;

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
                            if (entity.Has<TopDownControllerComponent>())
                                entity.Get<AnimationComponent>().animator.SetTrigger("Hit");

                            if (entity.Has<TopDownAiComponent>())
                            {
                                entity.Get<StompComponent>().delay = 0.4f;
                                entity.Get<TopDownAiComponent>().animation.SetTrigger("Hit");
                            }

                            Debug.Log($"Получил урон {changeHPComponent.Value}");
                        }
                        else
                        {
                            hpComponent.HP = 0;
                            entity.Get<DeathComponent>();
                            entity.Get<RagdollComponent>().Ragdoll.isActive = false;
                            if (entity.Has<TopDownControllerComponent>())
                            {
                                if (deathScreenFilter.IsEmpty()) return;
                                deathScreenFilter.Get1(default).deathScreen.gameObject.SetActive(true);
                                Cursor.visible = true;
                            }

                            if (entity.Has<TopDownAiComponent>())
                            {
                                entity.Get<DeathComponent>().deathTime = 5f;
                            }
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