using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client
{
    sealed class EcsInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        private EcsFilter<TopDownControllerComponent> topDownFilter = null;

        private InputActions inputMap;


        public void Init()
        {
            inputMap = new InputActions();
            inputMap.Player.Enable();
        }

        public void Run()
        {
            if (topDownFilter.IsEmpty()) return;
            ref var topDownComponent = ref topDownFilter.Get1(default);
            topDownComponent.InputMoveDirection = inputMap.Player.Move.ReadValue<Vector2>();
        }
    }

}