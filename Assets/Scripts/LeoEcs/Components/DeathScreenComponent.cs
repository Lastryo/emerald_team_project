using Leopotam.Ecs;
using Sirenix.OdinInspector;

namespace Client
{
    struct DeathScreenComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public DeathScreenUnityComponent deathScreen;
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<DeathScreenComponent>() = this;
            deathScreen.restartBtn.onClick.AddListener(() => { EcsStartup.World.NewEntity().Get<LoadGameEvent>(); });
            deathScreen.mainMenu.onClick.AddListener(() => { EcsStartup.World.NewEntity().Get<LoadMainMenuEvent>(); });
        }
    }
}