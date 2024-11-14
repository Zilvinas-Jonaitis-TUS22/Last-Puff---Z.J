using UnityEngine;
using UnityEngine.UI; // For the UI Slider component
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.1f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        [Header("Audio Sources")]
        public AudioSource walkAudioSource;
        public AudioSource runAudioSource;
        public AudioSource jumpAudioSource;
        public AudioSource landAudioSource;

        [Header("Sprint")]
        public Slider sprintBar; // UI Slider reference for sprint bar
        public float maxSprintDuration = 5.0f;
        public float sprintCooldown = 2.0f;
        private float _currentSprintDuration;
        private bool _isSprinting;
        private bool sprintKeyHeld = true;
        private float _sprintCooldownTimer = 0f;
        private bool _sprintReleased;

        private float _cinemachineTargetPitch;
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private bool _isJumping = false;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            // Initialize sprint duration and sprint bar
            _currentSprintDuration = maxSprintDuration;
            if (sprintBar != null)
            {
                sprintBar.maxValue = maxSprintDuration;
                sprintBar.value = _currentSprintDuration;
            }
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            UpdateSprintBar();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void UpdateSprintBar()
        {

            // Update the UI sprint bar with current sprint duration
            if (sprintBar != null)
            {
                sprintBar.value = _currentSprintDuration; // Normalize for the slider
            }
        }
        private void Move()
        {
            float targetSpeed = _isSprinting && _input.sprint && _currentSprintDuration > 0 ? SprintSpeed : MoveSpeed;

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            if (_input.move != Vector2.zero)
            {
                inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // Sprint logic
            if (_input.sprint && _currentSprintDuration > 0 && !sprintKeyHeld)
            {
                _sprintReleased = false;
                _isSprinting = true;
                _currentSprintDuration -= Time.deltaTime;
                // Play the run sound only if grounded
                if (runAudioSource != null && !runAudioSource.isPlaying && Grounded)
                {
                    runAudioSource.Play();
                    // Stop the walk sound if playing
                    if (walkAudioSource.isPlaying) walkAudioSource.Stop();
                }
            }
            else if (!_sprintReleased || (!_sprintReleased && _currentSprintDuration <= 0))
            {
                _sprintCooldownTimer = sprintCooldown;
                _sprintReleased = true;
                _isSprinting = false;
                runAudioSource.Stop();
            }
            else
            {
                _isSprinting = false;
                if (!walkAudioSource.isPlaying)
                {
                    walkAudioSource.Play();
                }
                if (_input.sprint)
                {
                    sprintKeyHeld = true;
                }
                else
                {
                    sprintKeyHeld = false;
                }
            }

            // Stop audio when airborne or when player is not moving
            if (!Grounded || _input.move == Vector2.zero)
            {
                walkAudioSource.Stop();
                runAudioSource.Stop();
            }

            // Continuous cooldown and gradual refill logic, regardless of movement
            if (!_isSprinting)
            {
                _sprintCooldownTimer -= Time.deltaTime;

                // Start refilling when cooldown finishes
                if (_sprintCooldownTimer <= 0 && _currentSprintDuration < maxSprintDuration)
                {
                    _currentSprintDuration += Time.deltaTime * 1.0f;  // Adjust refill rate here
                }

                // Ensure sprint duration doesn't exceed maximum
                if (_currentSprintDuration >= maxSprintDuration)
                {
                    _currentSprintDuration = maxSprintDuration;
                }
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    if (jumpAudioSource != null && !_isJumping)
                    {
                        jumpAudioSource.Play();
                        _isJumping = true;
                    }
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                _cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
                _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            bool wasGrounded = Grounded;
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (!wasGrounded && Grounded && landAudioSource != null)
            {
                landAudioSource.Play();
                _isJumping = false;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = Grounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }
    }
}
