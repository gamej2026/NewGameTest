using UnityEngine;

namespace Honbul
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        public float speed = GameConfig.WalkSpeed;
        public Transform cameraTransform;

        private CharacterController controller;
        private float verticalVelocity;

        private void Reset()
        {
            speed = GameConfig.WalkSpeed;
        }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            speed = GameConfig.WalkSpeed;
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 rawMove = GetCameraRelativeMove(horizontal, vertical);
            Vector3 move = rawMove.sqrMagnitude > 1f ? rawMove.normalized : rawMove;

            if (move.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
                float turnLerp = GameConfig.WalkSpeed * 4f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnLerp * Time.deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            verticalVelocity += GameConfig.Gravity * Time.deltaTime;

            Vector3 velocity = move * speed;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);
        }

        private Vector3 GetCameraRelativeMove(float horizontal, float vertical)
        {
            if (cameraTransform == null)
            {
                return new Vector3(horizontal, 0f, vertical);
            }

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            return forward * vertical + right * horizontal;
        }
    }
}