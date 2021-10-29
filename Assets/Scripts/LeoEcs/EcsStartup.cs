using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private EcsUiEmitter _uiEmitter;
        [SerializeField] private SceneManagementScriptable _sceneData;

        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystems _fixedSystems;

        public static EcsWorld World { get; private set; }
        public static EcsSystems Systems { get; private set; }
        public static EcsUiEmitter UiEmitter { get; private set; }

        public static EcsEntity NewEntity(string ID)
        {
            var entity = World.NewEntity();
            entity.Get<IDComponent>().id = ID;
#if UNITY_EDITOR
#endif
            return entity;
        }

        private void Awake()
        {
            // void can be switched to IEnumerator for support coroutines.
            _world = new EcsWorld();
            World = _world;
            _systems = new EcsSystems(_world);
            _fixedSystems = new EcsSystems(_world);

            Systems = _systems;
            UiEmitter = _uiEmitter;
            Initialize();
        }

        void Initialize()
        {
            // void can be switched to IEnumerator for support coroutines.


#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            _systems
            .Inject(_sceneData)
            .Add(new SceneLoadSystem())
            .Add(new EcsInputSystem())
            .Add(new SpawnSystem())
            .Add(new CameraFollowSystem())
            .Add(new AimSystem())
            .Add(new CrossbowBulletSystem())
            .Add(new ShootSystem())
            .Add(new DamageSystem())
            .Add(new HealthSystem())
            .Add(new SimpleAiSystem())




            .Add(new HPBarSystem())

                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register one-frame components (order is important), for example:
                // .OneFrame<TestComponent1> ()
                // .OneFrame<TestComponent2> ()

                // inject service instances here (order doesn't important), for example:
                // .Inject (new CameraService ())
                // .Inject (new NavMeshSupport ())
                .OneFrame<ResetEnemyAttackEvent>()
                .OneFrame<ChangeBulletEvent>()
                .OneFrame<DamageEvent>()
                .OneFrame<ShootInputEvent>()
                .OneFrame<ShootEvent>()
                .InjectUi(_uiEmitter)
                .Init();

            _fixedSystems
            .Add(new MovementSystem())
            .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void FixedUpdate()
        {
            _fixedSystems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}