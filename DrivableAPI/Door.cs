using UnityEngine;

namespace DrivableAPI
{
    public class Door : MonoBehaviour
    {
        internal float minAngle;
        internal float maxAngle;
        internal HingeJoint hinge;
        internal Rigidbody doorRB;
        internal float forceToAdd;

        bool usingDoor;
        bool doorClosed;

        private void Update()
        {
            if (DrivableAPI.raycastHit.collider == GetComponentInChildren<Collider>())
            {
                PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = true;
                if (Input.GetMouseButtonDown(0))
                {
                    if (doorClosed)
                    {
                        MasterAudio.PlaySound3DAndForget("CarFoley", transform, false, 1f, null, 0f, "open_door1");
                    }
                    usingDoor = true;
                }

                if (!Input.GetMouseButton(0))
                {
                    if (Quaternion.Dot(transform.localRotation, Quaternion.Euler(0, 0, 0)) < 0.999f)
                    {
                        doorClosed = false;
                    }
                    else
                    {
                        doorClosed = true;
                    }
                }
            }

            if (usingDoor)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!doorClosed)
                    {
                        doorRB.AddRelativeForce(forceToAdd * Time.deltaTime, 0, 0, ForceMode.Impulse);
                    }
                    else
                    {
                        JointLimits limits = hinge.limits;
                        limits.min = minAngle;
                        limits.max = maxAngle;
                        hinge.limits = limits;
                        doorRB.AddRelativeForce(-forceToAdd * Time.deltaTime, 0, 0, ForceMode.Impulse);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (Quaternion.Dot(transform.localRotation, Quaternion.Euler(0, 0, 0)) > 0.999f)
                    {
                        JointLimits limits = hinge.limits;
                        limits.min = minAngle;
                        limits.max = minAngle;
                        hinge.limits = limits;
                        MasterAudio.PlaySound3DAndForget("CarFoley", transform, false, 1f, null, 0f, "close_door1");
                    }
                    usingDoor = false;
                }
            }
        }
    }
}
