using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Client
{
    struct StopSpawnMarkerComponent : IComponent, IEcsIgnoreInFilter
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������� �������� ��...";
#endif
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<StopSpawnMarkerComponent>() = this;
        }
    }
}