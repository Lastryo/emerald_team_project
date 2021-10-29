using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct InAttackMarkerComponent : IComponent, IEcsIgnoreInFilter
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который накладывается на время атаки";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<InAttackMarkerComponent>() = this;
        }
    }
}