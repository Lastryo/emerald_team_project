using Leopotam.Ecs;

namespace Client
{
    sealed class DeathDeleteSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<DeathComponent> ecsFilter = null;

        public void Run()
        {
            if (ecsFilter.IsEmpty()) return;

            /*     foreach (var item in ecsFilter)
                 {
                     ref var death = ref ecsFilter.Get1(item);
                     ecsFilter.GetEntity();
                     if(death.currentTime >= death.deathTime)
                     {
                         death.GetEnti
                     }
                 }*/
        }
    }

}