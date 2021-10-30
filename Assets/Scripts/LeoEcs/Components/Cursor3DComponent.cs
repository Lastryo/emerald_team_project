using System;
using System.Linq;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct Cursor3DComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public Transform transform;
        [HideInInspector]
        public CursorColorData currentData;
        public CursorColorData[] cursorDatas;

        public void SetType(BulletType type)
        {
            currentData = cursorDatas.FirstOrDefault(x => x.Type == type);
            SetColor();
        }

        private void SetColor()
        {
            var renderers = transform.GetComponentsInChildren<Renderer>();
            foreach (var item in renderers)
            {
                item.material.SetColor("_BaseColor", currentData.Color);
            }
        }


        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<Cursor3DComponent>() = this;
            currentData = cursorDatas[0];
        }
    }

    [Serializable]
    public struct CursorColorData
    {
        public BulletType Type;
        public Color Color;
    }
}