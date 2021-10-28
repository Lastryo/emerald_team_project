using System.Security.Cryptography;
using Leopotam.Ecs;

namespace Client
{
    sealed class CameraFollowSystem : IEcsRunSystem
    {
        readonly EcsFilter<CameraComponent> cameraFilter;
        readonly EcsFilter<TopDownControllerComponent>.Exclude<FollowCameraComponent> characterTagFilter;

        public void Run()
        {
            if (cameraFilter.IsEmpty()) return;
            if (characterTagFilter.IsEmpty()) return;
            SetCamera();
        }

        public void SetCamera()
        {
            foreach (var characterTag in characterTagFilter)
            {
                ref var transformComponent = ref characterTagFilter.Get1(characterTag);
                foreach (var cameraIndex in cameraFilter)
                {
                    ref var cameraComponent = ref cameraFilter.Get1(cameraIndex);

                    if (!IsCameraFollowingNow(ref cameraComponent))
                    {
                        cameraComponent.virtualCamera.m_Follow = transformComponent.Transform;
                        cameraComponent.virtualCamera.m_LookAt = transformComponent.Transform;
                        ref var followComponent = ref characterTagFilter.GetEntity(characterTag).Get<FollowCameraComponent>();
                        followComponent.camera = cameraComponent.camera;
                    }

                    continue;
                }
            }
        }

        public bool IsCameraFollowingNow(ref CameraComponent follow)
        {
            return follow.virtualCamera.m_Follow != null;
        }
    }

}