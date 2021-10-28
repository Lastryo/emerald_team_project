using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct TopDownAiComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за управление врагом";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<TopDownAiComponent>() = this;
        }
    }
}