using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct CharacterHpBarMarkerComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<CharacterHpBarMarkerComponent>() = this;
        }
    }
}