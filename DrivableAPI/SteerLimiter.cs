using UnityEngine;

namespace DrivableAPI
{
    public class SteerLimiter : MonoBehaviour
    {
        private Rigidbody rigi;
        private Wheel[] frontWheels = new Wheel[2];
        public float maxSteeringAngle = 33f;
        public float minSteeringAngle = 2f;
        private float velocityDivider = 8.25f;

        private void Start()
        {
            rigi = GetComponent<Rigidbody>();
            frontWheels = GetComponent<Axles>().frontAxle.wheels;
        }

        private void FixedUpdate()
        {
            if (rigi == null || frontWheels == null) return;


            float speedFactor = rigi.velocity.magnitude / velocityDivider;
            float targetSteeringAngle = Mathf.Clamp(maxSteeringAngle / Mathf.Max(speedFactor, 1f), minSteeringAngle, maxSteeringAngle);

            foreach (var wheel in frontWheels)
            {
                if (wheel != null)
                    wheel.maxSteeringAngle = targetSteeringAngle;
            }
        }

        private void OnDisable()
        {
            if (frontWheels == null) return;

            foreach (var wheel in frontWheels)
            {
                if (wheel != null)
                    wheel.maxSteeringAngle = maxSteeringAngle;
            }
        }
    }
}