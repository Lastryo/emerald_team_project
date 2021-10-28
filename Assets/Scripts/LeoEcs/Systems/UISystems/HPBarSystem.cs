using Leopotam.Ecs;

namespace Client
{
    sealed class HPBarSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<HPBarComponent, CharacterHpBarMarkerComponent> characterBarFilter = null;
        readonly EcsFilter<TopDownControllerComponent, HPComponent> characterFilter = null;

        public void Run()
        {
            UpdateCharacterBar();
        }

        private void UpdateCharacterBar()
        {
            if (characterBarFilter.IsEmpty()) return;
            if (characterFilter.IsEmpty()) return;
            ref var hpComponent = ref characterFilter.Get2(default);
            ref var hpBarComponent = ref characterBarFilter.Get1(default);
            hpBarComponent.SetHP(hpComponent.HP, hpComponent.MaxHP);
        }
    }

}