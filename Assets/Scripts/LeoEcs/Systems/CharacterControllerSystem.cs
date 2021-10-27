using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
namespace Client
{
    sealed class CharacterControllerSystem : IEcsRunSystem
    {
        // auto-injected fields.
        readonly EcsWorld _world = null;

        readonly EcsFilter<TopDownControllerComponent> topDownControllerFilter = null;



        void IEcsRunSystem.Run()
        {

            if (topDownControllerFilter.IsEmpty()) return;
            foreach (var index in topDownControllerFilter)
            {
                ref var topDownComponent = ref topDownControllerFilter.Get1(index);
                ProcessUpdate(ref topDownComponent);
            }
        }

        private void ProcessUpdate(ref TopDownControllerComponent component)
        {
            if (component.Transform == null)
            {
                return;
            }

            if (!component.FreeMovement)
            {
                return;
            }

            component.newVelocity = component.Velocity;

            var positionLastFrame = component.Transform.position;

            AddInput(ref component);
            AddGravity(ref component);
            MoveWithPlatform(ref component);
            ComputeVelocityDelta(ref component);
            MoveCharacterController(ref component);
            DetectNewMovingPlatform(ref component);
            ComputeNewVelocity(ref component);
            ManualControllerColliderHit(ref component);
            HandleGroundContact(ref component);
        }

        /// <summary>
        /// Determines the new velocity based on the slope we're on and the input 
        /// </summary>
        private void AddInput(ref TopDownControllerComponent component)
        {
            if (component.Grounded && component.TooSteep())
            {
                component.idealVelocity.x = component.GroundNormal.x;
                component.idealVelocity.y = 0;
                component.idealVelocity.z = component.GroundNormal.z;
                component.idealVelocity = component.idealVelocity.normalized;
                component.idealDirection = Vector3.Project(component.InputMoveDirection, component.idealVelocity);
                component.idealVelocity = component.idealVelocity + (component.idealDirection * component.SlidingSpeedControl) + (component.InputMoveDirection - component.idealDirection) * component.SlidingDirectionControl;
                component.idealVelocity *= component.SlidingSpeed;
            }
            else
            {
                component.idealVelocity = component.CurrentMovement;
            }

            if (component.VelocityTransferMethod == TopDownControllerComponent.VelocityTransferOnJump.FloorVelocity)
            {
                component.idealVelocity += component.frameVelocity;
                component.idealVelocity.y = 0;
            }

            if (component.Grounded)
            {
                Vector3 sideways = Vector3.Cross(Vector3.up, component.idealVelocity);
                component.idealVelocity = Vector3.Cross(sideways, component.GroundNormal).normalized * component.idealVelocity.magnitude;
            }

            component.newVelocity = component.idealVelocity;
            component.newVelocity.y = component.Grounded ? Mathf.Min(component.newVelocity.y, 0) : component.newVelocity.y;
        }

        /// <summary>
        /// Adds the gravity to the new velocity and any AddedForce we may have
        /// </summary>
        private void AddGravity(ref TopDownControllerComponent component)
        {
            if (component.GravityActive)
            {
                if (component.Grounded)
                {
                    component.newVelocity.y = Mathf.Min(0, component.newVelocity.y) - component.Gravity * Time.deltaTime;
                }
                else
                {
                    component.newVelocity.y = component.Velocity.y - component.Gravity * Time.deltaTime;
                    component.newVelocity.y = Mathf.Max(component.newVelocity.y, -component.MaximumFallSpeed);
                }
            }
            component.newVelocity += component.AddedForce;
            component.AddedForce = Vector3.zero;
        }

        /// <summary>
        /// Moves and rotates the character controller to follow any moving platform we may be standing on
        /// </summary>
        private void MoveWithPlatform(ref TopDownControllerComponent component)
        {
            if (component.ShouldMoveWithPlatformThisFrame())
            {
                component.moveWithPlatformMoveDistance.x = component.moveWithPlatformMoveDistance.y = component.moveWithPlatformMoveDistance.z = 0f;
                component.moveWithPlatformGlobalPoint = component.movingPlatformCurrentHitCollider.TransformPoint(component.movingPlatformCurrentHitColliderLocal);
                component.moveWithPlatformMoveDistance = (component.moveWithPlatformGlobalPoint - component.movingPlatformCurrentGlobalPoint);
                if (component.moveWithPlatformMoveDistance != Vector3.zero)
                {
                    component.characterController.Move(component.moveWithPlatformMoveDistance);
                }
                component.moveWithPlatformGlobalRotation = component.movingPlatformCurrentHitCollider.rotation * component.movingPlatformLocalRotation;
                component.moveWithPlatformRotationDiff = component.moveWithPlatformGlobalRotation * Quaternion.Inverse(component.movingPlatformGlobalRotation);
                float yRotation = component.moveWithPlatformRotationDiff.eulerAngles.y;
                if (yRotation != 0)
                {
                    component.Transform.Rotate(0, yRotation, 0);
                }
            }
        }

        /// <summary>
        /// Computes the motion vector to apply to the character controller 
        /// </summary>
        private void ComputeVelocityDelta(ref TopDownControllerComponent component)
        {
            component.motion = component.newVelocity * Time.deltaTime;
            component.horizontalVelocityDelta.x = component.motion.x;
            component.horizontalVelocityDelta.y = 0f;
            component.horizontalVelocityDelta.z = component.motion.z;
            component.stickyOffset = Mathf.Max(component.characterController.stepOffset, component.horizontalVelocityDelta.magnitude);
            if (component.Grounded)
            {
                component.motion -= component.stickyOffset * Vector3.up;
            }
        }

        /// <summary>
        /// Moves the character controller by the computed component.motion 
        /// </summary>
        private void MoveCharacterController(ref TopDownControllerComponent component)
        {
            component.GroundNormal.x = component.GroundNormal.y = component.GroundNormal.z = 0f;

            component.collisionFlags = component.characterController.Move(component.motion); // controller move

            component.lastHitPoint = component.hitPoint;
            component.lastGroundNormal = component.GroundNormal;
        }

        /// <summary>
        /// Detects any moving platform we may be standing on
        /// </summary>
        private void DetectNewMovingPlatform(ref TopDownControllerComponent component)
        {
            if (component.movingPlatformCurrentHitCollider != component.movingPlatformHitCollider)
            {
                if (component.movingPlatformHitCollider != null)
                {
                    component.movingPlatformCurrentHitCollider = component.movingPlatformHitCollider;
                    component.lastMovingPlatformMatrix = component.movingPlatformHitCollider.localToWorldMatrix;
                    component.newMovingPlatform = true;
                }
            }
        }

        /// <summary>
        /// Determines the new Velocity value based on our position and our position last frame
        /// </summary>
        private void ComputeNewVelocity(ref TopDownControllerComponent component)
        {
            component.lastHorizontalVelocity.x = component.newVelocity.x;
            component.lastHorizontalVelocity.y = 0;
            component.lastHorizontalVelocity.z = component.newVelocity.z;

            if (Time.deltaTime != 0f)
            {
                component.Velocity = (component.Transform.position - component.positionLastFrame) / Time.deltaTime;
            }

            component.newHorizontalVelocity.x = component.Velocity.x;
            component.newHorizontalVelocity.y = 0;
            component.newHorizontalVelocity.z = component.Velocity.z;

            if (component.lastHorizontalVelocity == Vector3.zero)
            {
                component.Velocity.x = 0f;
                component.Velocity.z = 0f;
            }
            else
            {
                float newVelocity = Vector3.Dot(component.newHorizontalVelocity, component.lastHorizontalVelocity) / component.lastHorizontalVelocity.sqrMagnitude;
                component.Velocity = component.lastHorizontalVelocity * Mathf.Clamp01(newVelocity) + component.Velocity.y * Vector3.up;
            }
            if (component.Velocity.y < component.newVelocity.y - 0.001)
            {
                if (component.Velocity.y < 0)
                {
                    component.Velocity.y = component.newVelocity.y;
                }
            }

            component.Acceleration = (component.Velocity - component.VelocityLastFrame) / Time.deltaTime;
        }

        /// <summary>
        /// We handle ground contact, velocity transfer and moving platforms
        /// </summary>
        private void HandleGroundContact(ref TopDownControllerComponent component)
        {
            component.Grounded = component.characterController.isGrounded;

            if (component.Grounded && !component.IsGroundedTest())
            {
                component.Grounded = false;
                if ((component.VelocityTransferMethod == TopDownControllerComponent.VelocityTransferOnJump.InitialVelocity ||
                     component.VelocityTransferMethod == TopDownControllerComponent.VelocityTransferOnJump.FloorVelocity))
                {
                    component.frameVelocity = component.movingPlatformVelocity;
                    component.Velocity += component.movingPlatformVelocity;
                }
            }
            else if (!component.Grounded && component.IsGroundedTest())
            {
                component.Grounded = true;
                component.SubstractNewPlatformVelocity();
            }

            if (component.ShouldMoveWithPlatformThisFrame())
            {
                component.movingPlatformCurrentHitColliderLocal = component.movingPlatformCurrentHitCollider.InverseTransformPoint(component.movingPlatformCurrentGlobalPoint);
                component.movingPlatformGlobalRotation = component.Transform.rotation;
                component.movingPlatformLocalRotation = Quaternion.Inverse(component.movingPlatformCurrentHitCollider.rotation) * component.movingPlatformGlobalRotation;
            }

            component.ExitedTooSteepSlopeThisFrame = component.tooSteepLastFrame && !component.TooSteep();

            component.tooSteepLastFrame = component.TooSteep();
        }

        private void ManualControllerColliderHit(ref TopDownControllerComponent component)
        {
            component.smallestDistance = Single.MaxValue;
            component.longestDistance = Single.MinValue;
            component.smallestRaycast = component.emptyRaycast;

            // we cast 4 rays downwards to get ground normal
            float offset = component.characterController.radius;

            component.downRaycastsOffset.x = 0f;
            component.downRaycastsOffset.y = 0f;
            component.downRaycastsOffset.z = 0f;
            component.CastRayDownwards();
            component.downRaycastsOffset.x = -offset;
            component.downRaycastsOffset.y = offset;
            component.downRaycastsOffset.z = 0f;
            component.CastRayDownwards();
            component.downRaycastsOffset.x = 0f;
            component.downRaycastsOffset.y = offset;
            component.downRaycastsOffset.z = -offset;
            component.CastRayDownwards();
            component.downRaycastsOffset.x = offset;
            component.downRaycastsOffset.y = offset;
            component.downRaycastsOffset.z = 0f;
            component.CastRayDownwards();
            component.downRaycastsOffset.x = 0f;
            component.downRaycastsOffset.y = offset;
            component.downRaycastsOffset.z = offset;
            component.CastRayDownwards();

            // we handle our shortest ray
            if (component.smallestRaycast.collider != null)
            {
                float adjustedDistance = component.AdjustDistance(component.smallestRaycast.distance);

                if (component.smallestRaycast.normal.y > 0 && component.smallestRaycast.normal.y > component.GroundNormal.y)
                {
                    if (
                        (component.smallestRaycast.point.y - component.lastHitPoint.y < component.GroundNormalHeightThreshold)
                        && ((component.smallestRaycast.point != component.lastHitPoint)
                            || (component.lastGroundNormal == Vector3.zero)))
                    {
                        component.GroundNormal = component.smallestRaycast.normal;
                    }
                    else
                    {
                        component.GroundNormal = component.lastGroundNormal;
                    }
                    component.movingPlatformHitCollider = component.smallestRaycast.collider.transform;
                    component.hitPoint = component.smallestRaycast.point;
                    component.frameVelocity.x = component.frameVelocity.y = component.frameVelocity.z = 0f;
                }
            }

            // we cast a ray towards our move direction to handle pushing objects
            Physics.Raycast(component.Transform.position + component.characterController.center, component.CurrentMovement.normalized, out component.raycastDownHit,
                component.characterController.radius + component.characterController.skinWidth, component.ObstaclesLayerMask);

            if (component.raycastDownHit.collider != null)
            {
                component.HandlePush(component.raycastDownHit, component.raycastDownHit.point);
            }
        }


    }
}