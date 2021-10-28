using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct TopDownAiComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������� �������� �� ���������� ������";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<TopDownAiComponent>() = this;
        }
    }
}