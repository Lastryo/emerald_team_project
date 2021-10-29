using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct StompComponent : IComponent, IEcsIgnoreInFilter
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif

        public float delay;
        public float currentDelay;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<StompComponent>() = this;
        }
    }
}