using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client
{
    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "ScriptableObjects/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        public AssetReference reference;
    }
}