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

        public static EcsWorld World { get; private set; }
        public static EcsSystems Systems { get; private set; }
        public static EcsUiEmitter UiEmitter { get; private set; }

        public static EcsEntity NewEntity(string ID)
        {
            var entity = World.NewEntity();
            // entity.Get<IDComponent>().id = ID;
#if UNITY_EDITOR
#endif
            return entity;
        }

        void Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
            _systems
            .Inject(_sceneData)
            .InjectUi(_uiEmitter)
            .Add(new SceneLoadSystem())
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register one-frame components (order is important), for example:
                // .OneFrame<TestComponent1> ()
                // .OneFrame<TestComponent2> ()

                // inject service instances here (order doesn't important), for example:
                // .Inject (new CameraService ())
                // .Inject (new NavMeshSupport ())
                .Init();
        }

        void Update()
        {
            _systems?.Run();
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