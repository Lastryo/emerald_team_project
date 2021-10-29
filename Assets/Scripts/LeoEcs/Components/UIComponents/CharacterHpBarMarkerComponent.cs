using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct CharacterHpBarMarkerComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������� �������� ��...";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<CharacterHpBarMarkerComponent>() = this;
        }
    }
}