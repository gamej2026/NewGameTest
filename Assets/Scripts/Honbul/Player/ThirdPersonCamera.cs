using UnityEngine;

namespace Honbul
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;

        [SerializeField] private float pivotHeight = GameConfig.CameraHeight;
        [SerializeField] private float distance = GameConfig.CameraDistance;
        [SerializeField] private float mouseSensitivity = 180f;
        [SerializeField] private float minPitch = -25f;
        [SerializeField] private float maxPitch = 60f;
        [SerializeField] private float followSmoothing = 12f;

        private float yaw;
        private float pitch = 15f;

        private void Reset()
        {
            pivotHeight = GameConfig.CameraHeight;
            distance = GameConfig.CameraDistance;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 pivot = target.position + Vector3.up * pivotHeight;
            Vector3 desired = pivot - orbit * Vector3.forward * distance;

            Vector3 toDesired = desired - pivot;
            float targetDistance = distance;
            if (toDesired.sqrMagnitude > 0.0001f)
            {
                Vector3 direction = toDesired.normalized;
                int playerLayer = target.gameObject.layer;
                int mask = ~(1 << playerLayer);

                if (Physics.SphereCast(
                        pivot,
                        GameConfig.CameraCollisionRadius,
                        direction,
                        out RaycastHit hit,
                        distance,
                        mask,
                        QueryTriggerInteraction.Ignore))
                {
                    targetDistance = Mathf.Max(0.6f, hit.distance - 0.05f);
                }
            }

            Vector3 finalPos = pivot - orbit * Vector3.forward * targetDistance;
            transform.position = Vector3.Lerp(transform.position, finalPos, followSmoothing * Time.deltaTime);
            transform.LookAt(pivot);
        }
    }
}