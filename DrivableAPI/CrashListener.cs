using UnityEngine;

namespace DrivableAPI
{
    public class CrashListener : MonoBehaviour
    {
        private Rigidbody carRB;
        private Vector3 speed;
		GameObject death;
		internal string carCustomName;

		public bool usingSeatBelt;

        void Start()
        {
            carRB = GetComponent<Rigidbody>();
			death = GameObject.Find("Systems").transform.Find("Death").gameObject;
		}

		private void OnCollisionEnter(Collision col)
		{
			if (PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle").Value == carCustomName)
			{
				if (!death.activeSelf)
				{
					if (Vector3.Distance(carRB.velocity, speed) > (usingSeatBelt ? 70 : 40))
					{
						death.SetActive(true);
						death.GetComponent<PlayMakerFSM>().FsmVariables.FindFsmBool("Crash").Value = true;
					}
				}
			}
		}

		private void FixedUpdate()
		{
			speed = carRB.velocity;
		}
	}
}
