using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class AimSystem : IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        readonly EcsFilter<TopDownControllerComponent, FollowCameraComponent>.Exclude<DeathComponent> _characterFilter;
        readonly EcsFilter<Cursor3DComponent> cursor;
        readonly EcsFilter<AimComponent> uiAimFilter;
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

                if (cursor.IsEmpty()) return;
                {
                    ref var c = ref cursor.Get1(default);
                    c.transform.position = new Vector3(topDownComponent.FinalLookPosition.x, 2, topDownComponent.FinalLookPosition.z);
                }

                // if (uiAimFilter.IsEmpty()) return;
                // uiAimFilter.Get1(default).rect.position = followCameraComponent.camera.WorldToScreenPoint(topDownComponent.FinalLookPosition);
            }
        }

        private static Vector3 GetPosition(Vector2 mousePosition)
        {

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, LayerManager.MouseRaycastingLayerMask))
            {
                var hitPoint = hitInfo.point;
                hitPoint.y = 1;
                return hitPoint;
            }
            return Vector3.zero;
        }
    }
}