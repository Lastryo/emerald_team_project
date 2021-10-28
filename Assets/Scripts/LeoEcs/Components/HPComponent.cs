using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Client
{
    struct HPComponent : IComponent
    {
        public int HP;
        public int MaxHP;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif

        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<HPComponent>() = this;
        }
    }
}