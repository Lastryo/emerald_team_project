using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    struct BulletComponent : IComponent
    {
        public MeshRenderer bulletMesh;
        public Transform transform;

        public Vector3 direction;

        public float speed;

        public float coveredDistance;

        public void SetBulletColor(Color color)
        {
            bulletMesh.material.color = color;
        }

#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за...";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<BulletComponent>() = this;
        }
    }
}