using Leopotam.Ecs;

namespace Client
{
    public interface IComponent
    {
        void SetOwner(ref EcsEntity entity, out IComponent component);
    }
}
