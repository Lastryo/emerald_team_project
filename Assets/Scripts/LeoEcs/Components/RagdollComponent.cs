using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct RagdollComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif

        public TestRagdoll Ragdoll;

        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<RagdollComponent>() = this;
        }
    }
}