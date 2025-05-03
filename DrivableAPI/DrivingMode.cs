using System;
using UnityEngine;
using HutongGames.PlayMaker;

namespace DrivableAPI
{
    public class DrivingMode : MonoBehaviour
    {
        private Transform Player;
        public AxisCarController AxisCarController;
        public Drivetrain drivetrain;
        public Transform PlayerPivotObject;
        public string carCustomName;
        private bool InCar;

        private float enterexitCarCooldown;

        public Vector3 offset;

        public event EventHandler<EventArgs> OnEnterDrivingMode;
        public event EventHandler<EventArgs> OnExitDrivingMode;

        private FsmBool autoClutchBool;

        void Start()
        {
            Player = GameObject.Find("PLAYER").transform;

            autoClutchBool = GameObject.Find("Systems/Options").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("AutoClutch");

            AxisCarController.throttleAxis = "null";
            AxisCarController.brakeAxis = "null";
            AxisCarController.steerAxis = "null";
            AxisCarController.handbrakeAxis = "null";
            AxisCarController.clutchAxis = "null";
            AxisCarController.shiftUpButton = "null";
            AxisCarController.shiftDownButton = "null";
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == "PLAYER")
            {
                if (!InCar)
                {
                    if (enterexitCarCooldown <= 0)
                    {
                        PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive").Value = true;
                        PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "ENTER DRIVING MODE";
                        if (cInput.GetButtonDown("DrivingMode"))
                        {
                            other.gameObject.GetComponents<PlayMakerFSM>()[1].FsmVariables.GetFsmBool("PlayerInCar").Value = true;
                            PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerStop").Value = true;
                            PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "";
                            PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle").Value = carCustomName;
                            Player.GetComponent<CharacterController>().enabled = false;
                            Player.SetParent(PlayerPivotObject, true);
                            InCar = true;
                            AxisCarController.throttleAxis = "Throttle";
                            AxisCarController.brakeAxis = "Brake";
                            AxisCarController.steerAxis = "Horizontal";
                            AxisCarController.handbrakeAxis = "Handbrake";
                            AxisCarController.clutchAxis = "Clutch";
                            AxisCarController.shiftUpButton = "ShiftUp";
                            AxisCarController.shiftDownButton = "ShiftDown";
                            drivetrain.autoClutch = autoClutchBool.Value;
                            Player.transform.localRotation = Quaternion.Euler(new Vector3(0, Player.transform.localEulerAngles.y, 0));
                            enterexitCarCooldown = 0.5f;
                            OnEnterDrivingMode?.Invoke(this, new EventArgs());
                        }
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "PLAYER")
            {
                if (!InCar)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdrive").Value = false;
                    PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "";
                }
            }
        }

        void Update()
        {
            if (InCar)
            {
                Player.GetComponents<PlayMakerFSM>()[1].FsmVariables.GetFsmBool("PlayerInCar").Value = true;
                Player.transform.localPosition = new Vector3(Player.transform.localPosition.x + offset.x, offset.y, Player.transform.localPosition.z + offset.z);
                if (enterexitCarCooldown > 0)
                {
                    enterexitCarCooldown -= Time.deltaTime;
                }
                else
                {
                    if (cInput.GetButtonDown("DrivingMode"))
                    {
                        PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle").Value = "";
                        PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerStop").Value = false;
                        Player.SetParent(null);
                        Player.GetComponent<CharacterController>().enabled = true;
                        InCar = false;
                        AxisCarController.throttleAxis = "null";
                        AxisCarController.brakeAxis = "null";
                        AxisCarController.steerAxis = "null";
                        AxisCarController.handbrakeAxis = "null";
                        AxisCarController.clutchAxis = "null";
                        AxisCarController.shiftUpButton = "null";
                        AxisCarController.shiftDownButton = "null";
                        Player.transform.localRotation = Quaternion.Euler(new Vector3(0, Player.transform.localEulerAngles.y, 0));
                        enterexitCarCooldown = 0.5f;
                        OnExitDrivingMode?.Invoke(this, new EventArgs());
                    }
                }
            }
            else
            {
                if (enterexitCarCooldown > 0)
                {
                    enterexitCarCooldown -= Time.deltaTime;
                }
            }
        }
    }
}
