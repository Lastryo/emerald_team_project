using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class AimSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<TopDownControllerComponent, FollowCameraComponent>.Exclude<DeathComponent> _characterFilter;
        private Transform cube;



        public void Run()
        {
            if (_characterFilter.IsEmpty()) return;

            foreach (var index in _characterFilter)
            {
                ref var topDownComponent = ref _characterFilter.Get1(index);
                ref var followCameraComponent = ref _characterFilter.Get2(index);
                ref var characterEntity = ref _characterFilter.GetEntity(index);
                topDownComponent.FinalLookPosition = GetPosition(topDownComponent.InputLookDirection);
                topDownComponent.FinalLookPosition.y = topDownComponent.Transform.position.y;
                topDownComponent.ModelTransform.LookAt(topDownComponent.FinalLookPosition);

                if (cube == null)
                    cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;

                cube.position = topDownComponent.FinalLookPosition;
            }
        }

        private static Vector3 GetPosition(Vector2 mousePosition)
        {

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, LayerManager.MouseRaycastingLayerMask))
            {
                var hitPoint = hitInfo.point;
                hitPoint.y = 0;
                return hitPoint;
            }
            return Vector3.zero;
        }
    }
}