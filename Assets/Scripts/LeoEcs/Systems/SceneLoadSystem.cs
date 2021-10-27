using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    sealed class SceneLoadSystem : IEcsInitSystem, IEcsRunSystem
    {
        // auto-injected fields.

        readonly EcsWorld _world;
        readonly SceneManagementScriptable _sceneData;
        readonly EcsFilter<EcsUiClickEvent> _clickEvents = null;

        public void Init()
        {
            LoadMainMenu();
            // add your initialize code here.
        }

        private async void LoadMainMenu()
        {
            int c = SceneManager.sceneCount;
            for (int i = c - 1; i > 0; i--)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != "Main")
                    await _sceneData.UnloadScene(scene.name);
            }
            await _sceneData.LoadScene("Loading", LoadSceneMode.Additive);
            await _sceneData.LoadScene("MainMenu", LoadSceneMode.Additive);
            await _sceneData.UnloadScene("Loading");
        }

        private async void LoadNewGame()
        {
            int c = SceneManager.sceneCount;
            for (int i = c - 1; i > 0; i--)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != "Main")
                    await _sceneData.UnloadScene(scene.name);
            }
            await _sceneData.LoadScene("Loading", LoadSceneMode.Additive);
            await _sceneData.LoadScene("Level", LoadSceneMode.Additive, true);
            await _sceneData.UnloadScene("Loading");
        }

        public void Run()
        {
            foreach (var idx in _clickEvents)
            {
                ref var data = ref _clickEvents.Get1(idx);
                if (data.WidgetName == "new game")
                {
                    LoadNewGame();
                }

                if (data.WidgetName == "ToLobbyButton")
                {
                    LoadNewGame();
                }

                if (data.WidgetName == "ToMenuButton")
                {
                    LoadMainMenu();
                }
            }
        }
    }
}