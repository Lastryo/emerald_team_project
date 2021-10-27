using Leopotam.Ecs;

namespace Client
{
    public struct IDComponent : IComponent
    {
        public string id;

        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<IDComponent>() = this;
        }
    }
}