using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Client
{
    struct DeathComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public float deathTime;
        public float currentTime;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<DeathComponent>() = this;
        }
    }
}