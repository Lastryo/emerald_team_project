using System;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct CrossbowComponent : IComponent
    {
        public Transform point;
        public BulletData[] data;
        public Animator animation;

        [HideInInspector]
        public BulletType currentType;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за хранение пули";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            currentType = BulletType.Red;
            component = entity.Get<CrossbowComponent>() = this;
        }
    }

    [Serializable]
    public struct BulletData
    {
        public GameObject Bullet;
        public BulletType type;
    }
}