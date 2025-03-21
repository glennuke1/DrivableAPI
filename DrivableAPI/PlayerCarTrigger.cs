using UnityEngine;

namespace DrivableAPI
{
    internal class PlayerCarTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "PLAYER")
            {
                other.gameObject.GetComponents<PlayMakerFSM>()[1].FsmVariables.GetFsmBool("PlayerInCar").Value = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "PLAYER")
            {
                other.gameObject.GetComponents<PlayMakerFSM>()[1].FsmVariables.GetFsmBool("PlayerInCar").Value = false;
            }
        }
    }
}
