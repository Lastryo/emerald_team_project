using System;
using System.Collections;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Client
{
    struct TopDownControllerComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за настройки персонажа";
#endif

        public Transform Transform;
        public CharacterController characterController;
        /// the current input sent to this character
        [Tooltip("the current input sent to this character")]
        public Vector3 InputMoveDirection;

        /// the different possible update modes
        public enum UpdateModes { Update, FixedUpdate }
        /// the possible ways to transfer velocity on jump
        public enum VelocityTransferOnJump { NoTransfer, InitialVelocity, FloorVelocity, Relative }

        [Header("Settings")]
        /// whether the movement computation should occur at Update or FixedUpdate. FixedUpdate is the recommended choice.
        [Tooltip("whether the movement computation should occur at Update or FixedUpdate. FixedUpdate is the recommended choice.")]
        public UpdateModes UpdateMode;
        /// how the velocity should be affected when jumping from a moving ground
        [Tooltip("how the velocity should be affected when jumping from a moving ground")]
        public VelocityTransferOnJump VelocityTransferMethod;

        [Header("Raycasts")]

        /// the layer to consider as obstacles (will prevent movement)
        [Tooltip("the layer to consider as obstacles (will prevent movement)")]
        public LayerMask ObstaclesLayerMask;
        /// the length of the raycasts to cast downwards
        [Tooltip("the length of the raycasts to cast downwards")]
        public float GroundedRaycastLength;
        /// the distance to the ground beyond which the character isn't considered grounded anymore
        [Tooltip("the distance to the ground beyond which the character isn't considered grounded anymore")]
        public float MinimumGroundedDistance;

        [Header("Physics interaction")]

        /// the speed at which external forces get lerped to zero
        [Tooltip("the speed at which external forces get lerped to zero")]
        public float ImpactFalloff;
        /// the force to apply when colliding with rigidbodies
        [Tooltip("the force to apply when colliding with rigidbodies")]
        public float PushPower;
        /// a threshold against which to check when going over steps. Adjust that value if your character has issues going over small steps
        [Tooltip("a threshold against which to check when going over steps. Adjust that value if your character has issues going over small steps")]
        public float GroundNormalHeightThreshold;

        [Header("Movement")]

        /// the maximum vertical velocity the character can have while falling
        [Tooltip("the maximum vertical velocity the character can have while falling")]
        public float MaximumFallSpeed;
        /// the factor by which to multiply the speed while walking on a slope. x is the angle, y is the factor
        [Tooltip("the factor by which to multiply the speed while walking on a slope. x is the angle, y is the factor")]
        public AnimationCurve SlopeSpeedMultiplier;

        [Header("Steep Surfaces")]
        /// the current surface normal vector
        [Tooltip("the current surface normal vector")]
        public Vector3 GroundNormal;
        /// whether or not the character should slide while standing on steep surfaces
        [Tooltip("whether or not the character should slide while standing on steep surfaces")]
        public bool SlideOnSteepSurfaces;
        /// the speed at which the character should slide
        [Tooltip("the speed at which the character should slide")]
        public float SlidingSpeed;
        /// the control the player has on the speed while sliding down
        [Tooltip("the control the player has on the speed while sliding down")]
        public float SlidingSpeedControl;
        /// the control the player has on the direction while sliding down
        [Tooltip("the control the player has on the direction while sliding down")]
        public float SlidingDirectionControl;

        /// returns the center coordinate of the collider
        public Vector3 ColliderCenter { get { return Transform.position + characterController.center; } }
        /// returns the bottom coordinate of the collider
        public Vector3 ColliderBottom { get { return this.Transform.position + characterController.center + Vector3.down * characterController.bounds.extents.y; } }
        /// returns the top coordinate of the collider
        public Vector3 ColliderTop { get { return this.Transform.position + characterController.center + Vector3.up * characterController.bounds.extents.y; } }
        /// whether or not the character is sliding down a steep slope
        public bool IsSliding() { return (Grounded && SlideOnSteepSurfaces && TooSteep()); }
        /// whether or not the character is colliding above
        public bool CollidingAbove() { return (collisionFlags & CollisionFlags.CollidedAbove) != 0; }
        /// whether or not the current surface is too steep or not
        public bool TooSteep() { return (GroundNormal.y <= Mathf.Cos(characterController.slopeLimit * Mathf.Deg2Rad)); }
        /// whether or not the character just entered a slope/ground not too steep this frame
        public bool ExitedTooSteepSlopeThisFrame { get; set; }

        [Header("Gravity")]
        /// the current gravity to apply to our character (positive goes down, negative goes up, higher value, higher acceleration)
		[Tooltip("the current gravity to apply to our character (positive goes down, negative goes up, higher value, higher acceleration)")]
        public float Gravity;
        /// whether or not the gravity is currently being applied to this character
        [Tooltip("whether or not the gravity is currently being applied to this character")]
        public bool GravityActive;

        [Header("General Raycasts")]
        /// by default, the length of the raycasts used to get back to normal size will be auto generated based on your character's normal/standing height, but here you can specify a different value
        [Tooltip("by default, the length of the raycasts used to get back to normal size will be auto generated based on your character's normal/standing height, but here you can specify a different value")]
        public float CrouchedRaycastLengthMultiplier;
        /// if this is true, extra raycasts will be cast on all 4 sides to detect obstacles and feed the CollidingWithCardinalObstacle bool, only useful when working with grid movement, or if you need that info for some reason
        [Tooltip("if this is true, extra raycasts will be cast on all 4 sides to detect obstacles and feed the CollidingWithCardinalObstacle bool, only useful when working with grid movement, or if you need that info for some reason")]
        public bool PerformCardinalObstacleRaycastDetection;

        /// the current speed of the character
        [Tooltip("the current speed of the character")]
        public Vector3 Speed;
        /// the current velocity
        [Tooltip("the current velocity in units/second")]
        public Vector3 Velocity;
        /// the velocity of the character last frame
        [Tooltip("the velocity of the character last frame")]
        public Vector3 VelocityLastFrame;
        /// the current acceleration
        [Tooltip("the current acceleration")]
        public Vector3 Acceleration;
        /// whether or not the character is grounded
        [Tooltip("whether or not the character is grounded")]
        public bool Grounded;
        /// whether or not the character got grounded this frame
        [Tooltip("whether or not the character got grounded this frame")]
        public bool JustGotGrounded;
        /// the current movement of the character
        [Tooltip("the current movement of the character")]
        public Vector3 CurrentMovement;
        /// the direction the character is going in
        [Tooltip("the direction the character is going in")]
        public Vector3 CurrentDirection;
        /// the current friction
        [Tooltip("the current friction")]
        public float Friction;
        /// the current added force, to be added to the character's movement
        [Tooltip("the current added force, to be added to the character's movement")]
        public Vector3 AddedForce;
        /// whether or not the character is in free movement mode or not
        [Tooltip("whether or not the character is in free movement mode or not")]
        public bool FreeMovement;

        /// the object (if any) below our character
        public GameObject ObjectBelow { get; set; }
        public Vector3 AppliedImpact { get; set; }
        /// whether or not the character is on a moving platform
        public bool OnAMovingPlatform { get; set; }
        /// the speed of the moving platform
        public Vector3 MovingPlatformSpeed { get; set; }

        // the obstacle left to this controller (only updated if DetectObstacles is called)
        public GameObject DetectedObstacleLeft { get; set; }
        // the obstacle right to this controller (only updated if DetectObstacles is called)
        public GameObject DetectedObstacleRight { get; set; }
        // the obstacle up to this controller (only updated if DetectObstacles is called)
        public GameObject DetectedObstacleUp { get; set; }
        // the obstacle down to this controller (only updated if DetectObstacles is called)
        public GameObject DetectedObstacleDown { get; set; }
        // true if an obstacle was detected in any of the cardinal directions
        public bool CollidingWithCardinalObstacle { get; set; }

        [HideInInspector] public Vector3 positionLastFrame;
        [HideInInspector] public Vector3 speedComputation;
        [HideInInspector] public bool groundedLastFrame;
        [HideInInspector] public Vector3 impact;
        [HideInInspector] public float smallValue;

        [HideInInspector] public Vector3 lastGroundNormal;

        [HideInInspector] public Rigidbody pushedRigidbody;
        [HideInInspector] public Vector3 pushDirection;

        // moving platforms
        [HideInInspector] public Transform movingPlatformHitCollider;
        [HideInInspector] public Transform movingPlatformCurrentHitCollider;
        [HideInInspector] public Vector3 movingPlatformCurrentHitColliderLocal;
        [HideInInspector] public Vector3 movingPlatformCurrentGlobalPoint;
        [HideInInspector] public Quaternion movingPlatformLocalRotation;
        [HideInInspector] public Quaternion movingPlatformGlobalRotation;
        [HideInInspector] public Matrix4x4 lastMovingPlatformMatrix;
        [HideInInspector] public Vector3 movingPlatformVelocity;
        [HideInInspector] public bool newMovingPlatform;

        // char movement
        [HideInInspector] public CollisionFlags collisionFlags;
        [HideInInspector] public Vector3 frameVelocity;
        [HideInInspector] public Vector3 hitPoint;
        [HideInInspector] public Vector3 lastHitPoint;

        // velocity
        [HideInInspector] public Vector3 newVelocity;
        [HideInInspector] public Vector3 lastHorizontalVelocity;
        [HideInInspector] public Vector3 newHorizontalVelocity;
        [HideInInspector] public Vector3 motion;
        [HideInInspector] public Vector3 idealVelocity;
        [HideInInspector] public Vector3 idealDirection;
        [HideInInspector] public Vector3 horizontalVelocityDelta;
        [HideInInspector] public float stickyOffset;

        // collision detection
        [HideInInspector] public RaycastHit cardinalRaycast;
        [HideInInspector] public float smallestDistance;
        [HideInInspector] public float longestDistance;
        [HideInInspector] public RaycastHit smallestRaycast;
        [HideInInspector] public RaycastHit emptyRaycast;
        [HideInInspector] public Vector3 downRaycastsOffset;
        [HideInInspector] public Vector3 moveWithPlatformMoveDistance;
        [HideInInspector] public Vector3 moveWithPlatformGlobalPoint;
        [HideInInspector] public Quaternion moveWithPlatformGlobalRotation;
        [HideInInspector] public Quaternion moveWithPlatformRotationDiff;
        [HideInInspector] public RaycastHit raycastDownHit;
        [HideInInspector] public Vector3 raycastDownDirection;
        [HideInInspector] public RaycastHit canGoBackHeadCheck;
        [HideInInspector] public bool tooSteepLastFrame;


        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<TopDownControllerComponent>() = this;
        }

        /// <summary>
        /// Whether or not the character is grounded
        /// </summary>
        /// <returns></returns>
        public bool IsGroundedTest()
        {
            //return (_groundNormal.y > 0.01);

            bool grounded = false;

            if (smallestDistance <= MinimumGroundedDistance)
            {
                grounded = (GroundNormal.y > 0.01);
            }

            return grounded;
        }

        /// <summary>
        /// Computes the relative velocity
        /// </summary>
        /// <returns></returns>
        public IEnumerator SubstractNewPlatformVelocity()
        {
            if ((VelocityTransferMethod == VelocityTransferOnJump.InitialVelocity ||
                 VelocityTransferMethod == VelocityTransferOnJump.FloorVelocity))
            {
                if (newMovingPlatform)
                {
                    Transform platform = movingPlatformCurrentHitCollider;
                    yield return new WaitForFixedUpdate();
                    if (Grounded && platform == movingPlatformCurrentHitCollider)
                    {
                        yield return 1;
                    }
                }
                Velocity -= movingPlatformVelocity;
            }
        }

        /// <summary>
        /// Whether or not our character should move with the moving platform this frame
        /// </summary>
        /// <returns></returns>
        public bool ShouldMoveWithPlatformThisFrame()
        {
            return (
                (Grounded || VelocityTransferMethod == VelocityTransferOnJump.Relative)
                && movingPlatformCurrentHitCollider != null
            );
        }

        /// <summary>
        /// Casts a ray downwards and adjusts distances if needed
        /// </summary>
        public void CastRayDownwards()
        {
            if (smallestDistance <= MinimumGroundedDistance)
            {
                return;
            }

            Physics.Raycast(this.Transform.position + characterController.center + downRaycastsOffset, raycastDownDirection, out raycastDownHit,
                characterController.height / 2f + GroundedRaycastLength, ObstaclesLayerMask);

            if (raycastDownHit.collider != null)
            {
                float adjustedDistance = AdjustDistance(raycastDownHit.distance);

                if (adjustedDistance < smallestDistance) { smallestDistance = adjustedDistance; smallestRaycast = raycastDownHit; }
                if (adjustedDistance > longestDistance) { longestDistance = adjustedDistance; }
            }
        }

        /// <summary>
        /// Returns the real distance between the extremity of the character and the ground
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public float AdjustDistance(float distance)
        {
            float adjustedDistance = distance - characterController.height / 2f -
                                     characterController.skinWidth;
            return adjustedDistance;
        }

        /// <summary>
        /// Adds a force to the colliding object at the hit position, to interact with the physics world
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="hitPosition"></param>
        public void HandlePush(RaycastHit hit, Vector3 hitPosition)
        {
            pushedRigidbody = hit.collider.attachedRigidbody;

            if ((pushedRigidbody == null) || (pushedRigidbody.isKinematic))
            {
                return;
            }

            pushDirection.x = CurrentMovement.normalized.x;
            pushDirection.y = 0;
            pushDirection.z = CurrentMovement.normalized.z;

            pushedRigidbody.AddForceAtPosition(pushDirection * PushPower, hitPosition);
        }
    }
}