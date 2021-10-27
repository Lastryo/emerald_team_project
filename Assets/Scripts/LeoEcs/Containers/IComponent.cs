using Leopotam.Ecs;

namespace Client
{
    public interface IComponent
    {
        void SetOwner(in EcsEntity entity);
    }
}
