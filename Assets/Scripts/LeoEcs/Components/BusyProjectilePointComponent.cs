using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct BusyProjectilePointComponent : IComponent, IEcsIgnoreInFilter
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������ ������� ������� ����� ������� �������";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<BusyProjectilePointComponent>() = this;
        }
    }
}