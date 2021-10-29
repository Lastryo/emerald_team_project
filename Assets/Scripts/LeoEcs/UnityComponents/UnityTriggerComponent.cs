using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class UnityTriggerComponent : MonoBehaviour
    {
        [HideInInspector]
        public EcsEntity Entity { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            ref var triggerComponent = ref Entity.Get<TriggerComponent>();
            other.TryGetComponent<ActorContainer>(out var actorContainer);
            if ((triggerComponent.Mask.value & (1 << other.transform.gameObject.layer)) > 0)
                if (triggerComponent.InActions != null && triggerComponent.InActions.Count > 0)
                    foreach (var action in triggerComponent.InActions)
                    {
                        action?.Invoke(Entity, actorContainer != null ? actorContainer.Entity : EcsEntity.Null);
                    }

        }

        private void OnTriggerExit(Collider other)
        {
            ref var triggerComponent = ref Entity.Get<TriggerComponent>();
            other.TryGetComponent<ActorContainer>(out var actorContainer);
            if ((triggerComponent.Mask.value & (1 << other.transform.gameObject.layer)) > 0)
                if (triggerComponent.OutActions != null && triggerComponent.OutActions.Count > 0)
                    foreach (var action in triggerComponent.OutActions)
                        action?.Invoke(Entity, actorContainer != null ? actorContainer.Entity : EcsEntity.Null);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}