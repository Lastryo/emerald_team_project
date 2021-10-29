using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct EnemyTypeComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif

        public BulletType Type;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<EnemyTypeComponent>() = this;
        }

    }
}