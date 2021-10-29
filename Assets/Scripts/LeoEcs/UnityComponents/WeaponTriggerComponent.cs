using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class WeaponTriggerComponent : MonoBehaviour, IHaveEntity
    {
        public EcsEntity Entity { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            ref var triggerComponent = ref Entity.Get<AttackTriggerComponent>();
            if ((triggerComponent.Mask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (other.TryGetComponent<ActorContainer>(out var actorContainer))
                {
                    ref var hpComponent = ref actorContainer.Entity.Get<ChangeHPComponent>();
                    hpComponent.Type = ChangeHPComponent.ChangeHealthType.Damage;
                    hpComponent.Value = Entity.Get<TopDownAiComponent>().damage;
                }
            }
        }
    }
}