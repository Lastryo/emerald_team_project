using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class UnityTriggerComponent : MonoBehaviour
    {
        public EcsEntity Entity { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            ref var triggerComponent = ref Entity.Get<TriggerComponent>();
            if ((triggerComponent.Mask.value & (1 << other.transform.gameObject.layer)) > 0)
                if (triggerComponent.InActions != null && triggerComponent.InActions.Count > 0)
                    foreach (var action in triggerComponent.InActions)
                        action?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            ref var triggerComponent = ref Entity.Get<TriggerComponent>();
            if ((triggerComponent.Mask.value & (1 << other.transform.gameObject.layer)) > 0)
                if (triggerComponent.OutActions != null && triggerComponent.OutActions.Count > 0)
                    foreach (var action in triggerComponent.OutActions)
                        action?.Invoke();
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