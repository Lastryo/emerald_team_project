using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Client
{
    struct MovementComponent : IComponent
    {
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за передвижение";
#endif

        /// the possible rotation modes for the character
        public enum Movements { Free, Strict2DirectionsHorizontal, Strict2DirectionsVertical, Strict4Directions, Strict8Directions }

        /// the current reference movement speed
        public float MovementSpeed { get; set; }
        /// if this is true, movement will be forbidden (as well as flip)
        public bool MovementForbidden { get; set; }

        [Header("Direction")]

        /// whether the character can move freely, in 2D only, in 4 or 8 cardinal directions
        [Tooltip("whether the character can move freely, in 2D only, in 4 or 8 cardinal directions")]
        public Movements Movement;

        [Header("Settings")]

        /// whether or not movement input is authorized at that time
        [Tooltip("whether or not movement input is authorized at that time")]
        public bool InputAuthorized;
        /// whether or not input should be analog
        [Tooltip("whether or not input should be analog")]
        public bool AnalogInput;
        /// whether or not input should be set from another script
        [Tooltip("whether or not input should be set from another script")]
        public bool ScriptDrivenInput;

        [Header("Speed")]

        /// the speed of the character when it's walking
        [Tooltip("the speed of the character when it's walking")]
        public float WalkSpeed;
        /// whether or not this component should set the controller's movement
		[Tooltip("whether or not this component should set the controller's movement")]
        public bool ShouldSetMovement;
        /// the speed threshold after which the character is not considered idle anymore
        [Tooltip("the speed threshold after which the character is not considered idle anymore")]
        public float IdleThreshold;

        [Header("Acceleration")]

        /// the acceleration to apply to the current speed / 0f : no acceleration, instant full speed
        [Tooltip("the acceleration to apply to the current speed / 0f : no acceleration, instant full speed")]
        public float Acceleration;
        /// the deceleration to apply to the current speed / 0f : no deceleration, instant stop
        [Tooltip("the deceleration to apply to the current speed / 0f : no deceleration, instant stop")]
        public float Deceleration;
        /// whether or not to interpolate movement speed
        [Tooltip("whether or not to interpolate movement speed")]
        public bool InterpolateMovementSpeed;
        /// the multiplier to apply to the horizontal movement
        public float MovementSpeedMultiplier { get; set; }

        [Header("Walk Feedback")]
        /// the particles to trigger while walking
        [Tooltip("the particles to trigger while walking")]
        public ParticleSystem[] WalkParticles;

        [Header("Touch The Ground Feedback")]
        /// the particles to trigger when touching the ground
        [Tooltip("the particles to trigger when touching the ground")]
        public ParticleSystem[] TouchTheGroundParticles;
        /// the sfx to trigger when touching the ground
        [Tooltip("the sfx to trigger when touching the ground")]
        public AudioClip[] TouchTheGroundSfx;

        [HideInInspector] public float movementSpeed;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public Vector3 movementVector;
        [HideInInspector] public Vector2 currentInput;
        [HideInInspector] public Vector2 normalizedInput;
        [HideInInspector] public Vector2 lerpedInput;
        [HideInInspector] public float acceleration;
        [HideInInspector] public bool walkParticlesPlaying;

        [HideInInspector] public string speedAnimationParameterName;// = "Speed";
        [HideInInspector] public string walkingAnimationParameterName;// = "Walking";
        [HideInInspector] public string idleAnimationParameterName;// = "Idle";
        [HideInInspector] public int speedAnimationParameter;
        [HideInInspector] public int walkingAnimationParameter;
        [HideInInspector] public int idleAnimationParameter;
        [HideInInspector] public bool isInit;
        public void SetOwner(in EcsEntity entity)
        {
            entity.Get<MovementComponent>() = this;
        }
    }
}