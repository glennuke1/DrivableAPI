using UnityEngine;
using System.Linq;

namespace DrivableAPI
{
    public class CrashListener : MonoBehaviour
    {
        internal bool HealthModPresent;
        private Rigidbody carRB;
        private Vector3 speed;
		GameObject death;
		internal string carCustomName;

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
					if (Vector3.Distance(carRB.velocity, speed) > 30)
					{
						if (HealthModPresent)
						{
							var mod = MSCLoader.ModLoader.LoadedMods.FirstOrDefault((MSCLoader.Mod x) => x.ID == "Health");
							if (mod != null)
							{
								var method = mod.GetType().GetMethod("damage");
								if (method != null)
								{
									object[] parameters = { Vector3.Distance(carRB.velocity, speed) };
									bool damage = (bool)method.Invoke(mod, parameters);
									if (damage)
									{
										death.SetActive(true);
										death.GetComponent<PlayMakerFSM>().FsmVariables.FindFsmBool("Crash").Value = true;
									}
								}
							}
							return;
						}

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
