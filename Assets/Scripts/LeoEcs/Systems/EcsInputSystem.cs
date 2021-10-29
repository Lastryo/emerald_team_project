using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client
{
    sealed class EcsInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        private EcsFilter<TopDownControllerComponent> topDownFilter = null;
        private EcsFilter<BulletComponent>.Exclude<BulletShootingComponent> isReadyShoot = null;

        private InputActions inputMap;


        public void Init()
        {
            inputMap = new InputActions();
            inputMap.Player.Enable();
            inputMap.Player.Fire.performed += InputShoot;
            inputMap.Player.SilverBullet.performed += InputSilverBullet;
            inputMap.Player.RedBullet.performed += InputRedBullet;
            inputMap.Player.YellowBullet.performed += InputYellowBullet;
            inputMap.Player.VioletBullet.performed += InputVioletBullet;
        }

        public void Run()
        {
            if (topDownFilter.IsEmpty()) return;
            ref var topDownComponent = ref topDownFilter.Get1(default);
            topDownComponent.InputMoveDirection = inputMap.Player.Move.ReadValue<Vector2>();
            topDownComponent.InputLookDirection = inputMap.Player.Look.ReadValue<Vector2>();


        }

        public void InputShoot(InputAction.CallbackContext context)
        {
            if (isReadyShoot.IsEmpty()) return;
            _world.NewEntity().Get<ShootInputEvent>();
        }

        public void InputRedBullet(InputAction.CallbackContext context)
        {
            _world.NewEntity().Get<ChangeBulletEvent>().Type = BulletType.Red;
        }

        public void InputYellowBullet(InputAction.CallbackContext context)
        {
            _world.NewEntity().Get<ChangeBulletEvent>().Type = BulletType.Yellow;
        }

        public void InputVioletBullet(InputAction.CallbackContext context)
        {
            _world.NewEntity().Get<ChangeBulletEvent>().Type = BulletType.Violet;
        }

        public void InputSilverBullet(InputAction.CallbackContext context)
        {
            _world.NewEntity().Get<ChangeBulletEvent>().Type = BulletType.Silver;
        }
    }

}