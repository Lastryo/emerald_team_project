using Leopotam.Ecs;

namespace Client
{
    public struct IDComponent : IComponent
    {
        public string id;

        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<IDComponent>() = this;
        }
    }
}