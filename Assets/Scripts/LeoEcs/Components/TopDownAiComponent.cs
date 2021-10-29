using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    struct TopDownAiComponent : IComponent
    {
        public NavMeshAgent agent;
        public Transform transform;
        public float attackRange;
        public int damage;


#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "��������� ������� �������� �� ���������� ������";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<TopDownAiComponent>() = this;
        }
    }
}