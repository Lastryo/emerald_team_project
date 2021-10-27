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
    //public AssetReference scene;
    public SceneData[] scenes;

    public async Task LoadScene(string sceneName, LoadSceneMode mode, bool isMain = false)
    {
        var neededScene = scenes.FirstOrDefault(s => s.name == sceneName);
        var asset = await neededScene.sceneAsset.LoadSceneAsync(mode, true).Task;
        if (isMain)
            SceneManager.SetActiveScene(asset.Scene);
    }

    public Task UnloadScene(string sceneName)
    {
        var neededScene = scenes.FirstOrDefault(s => s.name == sceneName);
        return neededScene.sceneAsset.UnLoadScene().Task;
    }

    [Serializable]
    public struct SceneData
    {
#if UNITY_EDITOR
        private void AddName()
        {
            name = sceneAsset.editorAsset.name;
        }
        [OnValueChanged("AddName")]
#endif
        public AssetReference sceneAsset;

        [ReadOnly]
        public string name;


    }
}
