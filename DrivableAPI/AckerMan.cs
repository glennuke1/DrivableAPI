using UnityEngine;

namespace DrivableAPI
{
    public class AckerMan : MonoBehaviour
    {
        public AxisCarController ACC;
        private Transform AckerFL;
        private Transform AckerFR;
        public float mult = -7.3f;
        public float overall;

        private void Start()
        {
            ACC = GetComponent<AxisCarController>();
            AckerFL = GetComponent<Axles>().frontAxle.leftWheel.transform.GetChild(0);
            AckerFR = GetComponent<Axles>().frontAxle.rightWheel.transform.GetChild(0);
        }

        private void Update()
        {
            overall = ACC.steering * mult;
            AckerFL.localEulerAngles = new Vector3(0.0f, Mathf.Clamp(overall, mult, 0.0f), 0.0f);
            AckerFR.localEulerAngles = new Vector3(0.0f, Mathf.Clamp(overall, 0.0f, -mult), 0.0f);
        }
    }
}