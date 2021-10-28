using Client;
using Leopotam.Ecs;
using UnityEngine;

sealed class MovementSystem : IEcsRunSystem
{
    // auto-injected fields.
    readonly EcsWorld _world = null;
    private EcsFilter<TopDownControllerComponent> characterFilter = null;
    private int animationMoveId = Animator.StringToHash("Forward");

    public void Run()
    {
        if (characterFilter.IsEmpty()) return;
        foreach (var index in characterFilter)
        {
            ref var entity = ref characterFilter.GetEntity(index);
            ref var request = ref characterFilter.Get1(index);
            if (!entity.Has<FollowCameraComponent>()) return;
            ref var camera = ref entity.Get<FollowCameraComponent>();
            var desiredMoveDirection = CustomCameraExtension.GetCameraDirection(camera.camera.transform, request.InputMoveDirection);
            if (desiredMoveDirection.sqrMagnitude > 0.1f)
                Move(desiredMoveDirection);
            else Stop();
        }
    }

    private void Move(Vector3 moveDirection)
    {
        foreach (var character in characterFilter)
        {
            ref var moveComponent = ref characterFilter.Get1(character);
            ref var characterEntity = ref characterFilter.GetEntity(character);
            if (!moveComponent.isMoving)
            {
                moveComponent.isMoving = true;
                //_world.NewEntity().Get<MoveAnimationEvent>().IsActive = moveComponent.isMoving;
            }


            var agent = moveComponent.Agent;
            var transform = moveComponent.Transform;

            agent.Move(moveDirection * moveComponent.MoveSpeed * Time.deltaTime);
            if (characterEntity.Has<AnimationComponent>())
            {
                characterEntity.Get<AnimationComponent>().animator.SetFloat(animationMoveId, moveComponent.InputMoveDirection.sqrMagnitude);
            }

            moveComponent.isMoving = true;
            // Rotate(transform, moveDirection);
        }
    }

    private void Rotate(Transform transform, Vector3 moveDirection)
    {
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }


    private void Stop()
    {
        foreach (var character in characterFilter)
        {
            ref var moveComponent = ref characterFilter.Get1(character);
            ref var characterEntity = ref characterFilter.GetEntity(character);
            if (characterEntity.Has<AnimationComponent>())
            {
                characterEntity.Get<AnimationComponent>().animator.SetFloat(animationMoveId, moveComponent.InputMoveDirection.sqrMagnitude);
            }
            moveComponent.isMoving = false;
        }
    }

    private void CheckAndAnimateMove(bool value)
    {
        foreach (var character in characterFilter)
        {
            ref var moveComponent = ref characterFilter.Get1(character);
            if (moveComponent.isMoving == value) continue;

            //_world.NewEntity().Get<MoveAnimationEvent>().IsActive = moveComponent.isMoving = value;
        }
    }
}