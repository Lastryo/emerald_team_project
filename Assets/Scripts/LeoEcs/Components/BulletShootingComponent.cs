using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct BulletShootingComponent : IComponent, IEcsIgnoreInFilter
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<BulletShootingComponent>() = this;
        }
    }
}