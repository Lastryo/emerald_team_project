using System;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneManagement")]
public class SceneManagementScriptable : ScriptableObject
{
    public SceneData[] scenes;

    public async Task<SceneInstance> LoadScene(SceneType type, LoadSceneMode mode, bool isMain = false)
    {
        var neededScene = scenes.FirstOrDefault(s => s.type == type);
        var asset = await neededScene.sceneAsset.LoadSceneAsync(mode, true).Task;
        if (isMain)
        {
            SceneManager.SetActiveScene(asset.Scene);
        }
        return asset;
    }

    public void SetActiveScene(Scene scene)
    {
        SceneManager.SetActiveScene(scene);
    }

    public Task UnloadScene(SceneType type)
    {
        var neededScene = scenes.FirstOrDefault(s => s.type == type);
        return neededScene.sceneAsset.UnLoadScene().Task;
    }

    public async Task UnloadScenes()
    {
        foreach (var item in scenes)
        {
            if (item.sceneAsset.IsValid())
                await item.sceneAsset.UnLoadScene().Task;
        }
    }

    [Serializable]
    public struct SceneData
    {
        public AssetReference sceneAsset;
        public SceneType type;
    }

    public enum SceneType
    {

        Loading,
        MainMenu,
        Game
    }
}
