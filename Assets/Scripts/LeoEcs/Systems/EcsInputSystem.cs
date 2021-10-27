using Leopotam.Ecs;
using UnityEngine.InputSystem;

namespace Client
{
    sealed class EcsInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld _world = null;

        private InputActions inputMap;

        public void Init()
        {
            inputMap = new InputActions();
            inputMap.Player.Enable();
        }

        public void Run()
        {

        }
    }

}