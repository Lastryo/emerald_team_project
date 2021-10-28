using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct BusyProjectilePointComponent : IComponent, IEcsIgnoreInFilter
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент маркер который наложен когда арбалет заряжен";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<BusyProjectilePointComponent>() = this;
        }
    }
}