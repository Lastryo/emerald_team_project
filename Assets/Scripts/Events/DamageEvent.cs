using Leopotam.Ecs;

namespace Client
{
    public struct DamageEvent
    {
        public EcsEntity damagedEntity { get; set; }
        public EcsEntity damagerEntity { get; set; }
    }
}