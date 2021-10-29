using System;
using System.Threading.Tasks;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneManagementScriptable;

namespace Client
{
    sealed class SceneLoadSystem : IEcsInitSystem, IEcsRunSystem
    {
        // auto-injected fields.

        readonly EcsWorld _world;
        readonly SceneManagementScriptable _sceneData;
        readonly EcsFilter<EcsUiClickEvent> _clickEvents = null;

        readonly EcsFilter<LoadMainMenuEvent> _mainMenuEvent = null;

        readonly EcsFilter<LoadGameEvent> _gameEvent = null;


        public void Init()
        {
            LoadMainMenu();
            // add your initialize code here.
        }

        private async void LoadMainMenu()
        {
            await _sceneData.UnloadScenes();

            await _sceneData.LoadScene(SceneType.Loading, LoadSceneMode.Additive);
            await _sceneData.LoadScene(SceneType.MainMenu, LoadSceneMode.Additive);
            await _sceneData.UnloadScene(SceneType.Loading);
        }

        private async void LoadNewGame()
        {
            await _sceneData.UnloadScenes();

            await _sceneData.LoadScene(SceneType.Loading, LoadSceneMode.Additive);
            await _sceneData.LoadScene(SceneType.Game, LoadSceneMode.Additive, true);
            await Task.Delay(TimeSpan.FromSeconds(2));
            await _sceneData.UnloadScene(SceneType.Loading);
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

            if (!_mainMenuEvent.IsEmpty())
            {
                LoadMainMenu();
            }

            if (!_gameEvent.IsEmpty())
            {
                LoadNewGame();
            }


        }
    }
}