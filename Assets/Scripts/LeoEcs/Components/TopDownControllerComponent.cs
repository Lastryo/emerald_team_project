using System;
using System.Collections;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    struct TopDownControllerComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за настройки персонажа";
#endif
        public NavMeshAgent Agent;
        public Transform Transform;
        public Collider Collider;
        public Transform ModelTransform;
        public float MoveSpeed;

        [HideInInspector]
        public Vector2 InputMoveDirection;

        [HideInInspector]
        public Vector2 InputLookDirection;

        [HideInInspector]
        public Vector2 InputDeltaLookDirection;

        [HideInInspector]
        public Vector3 FinalLookPosition;

        [HideInInspector]
        public bool isMoving;
        public void SetOwner(in EcsEntity entity, out IComponent component)
        {
            component = entity.Get<TopDownControllerComponent>() = this;
        }
    }
}