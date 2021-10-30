using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class DeathDeleteSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<DeathComponent> ecsFilter = null;

        public void Run()
        {
            if (ecsFilter.IsEmpty()) return;

            foreach (var item in ecsFilter)
            {
                ref var death = ref ecsFilter.Get1(item);
                ref var entity = ref ecsFilter.GetEntity(item);

                if (entity.Has<TopDownAiComponent>())
                {
                    if (death.currentTime >= death.deathTime)
                    {
                        GameObject.Destroy(entity.Get<TopDownAiComponent>().transform.gameObject);
                        entity.Destroy();
                        continue;
                    }
                    else
                    {
                        death.currentTime += Time.deltaTime;
                    }

                }

            }
        }
    }

}