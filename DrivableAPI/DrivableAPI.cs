using MSCLoader;
using UnityEngine;

namespace DrivableAPI
{
    public class DrivableAPI
    {
        public static DrivingMode AddDrivingMode(GameObject carRoot, string carCustomName, Vector3 drivingModeOffset)
        {
            Transform playerTrigger = carRoot.transform.Find("PlayerTrigger");

            if (playerTrigger == null)
            {
                ModConsole.Log("[Drivable API] Please set up PlayerTrigger inside unity");
                return null;
            }

            playerTrigger.gameObject.layer = LayerMask.NameToLayer("PlayerOnlyColl");
            Transform DriveTrigger = playerTrigger.Find("DriveTrigger");
            
            if (DriveTrigger == null)
            {
                ModConsole.Log("[Drivable API] Please set up DriveTrigger under PlayerTrigger inside unity");
                return null;
            }

            try
            {
                DriveTrigger.gameObject.layer = LayerMask.NameToLayer("PlayerOnlyColl");

                playerTrigger.gameObject.AddComponent<PlayerCarTrigger>();

                DrivingMode drivingMode = DriveTrigger.gameObject.AddComponent<DrivingMode>();
                drivingMode.offset = drivingModeOffset;
                drivingMode.AxisCarController = carRoot.GetComponent<AxisCarController>();
                drivingMode.drivetrain = carRoot.GetComponent<Drivetrain>();
                drivingMode.PlayerPivotObject = DriveTrigger;
                drivingMode.carCustomName = carCustomName;
                return drivingMode;
            }
            catch (System.Exception ex)
            {
                Debug.Log("[DrivableAPI] Exception in AddDrivingMode() report this to mod developer: " + ex.Message);
                return null;
            }
        }

        public static CrashListener AddCrashListener(GameObject carRoot, string carCustomName)
        {
            if (!carRoot.GetComponent<Rigidbody>())
            {
                ModConsole.Log($"[Drivable API] {carRoot} does not have a rigidbody!");
                return null;
            }

            CrashListener crashListener = carRoot.AddComponent<CrashListener>();
            crashListener.carCustomName = carCustomName;

            return crashListener;
        }

        public static void AddGearIndicator(GameObject carRoot, string carCustomName, bool automatic)
        {
            GearIndicator gearIndicator = carRoot.AddComponent<GearIndicator>();
            gearIndicator.carCustomName = carCustomName;
            gearIndicator.automatic = automatic;
        }

        public static void AddSteerLimiter(GameObject carRoot, float maxSteeringAngle, float minSteeringAngle)
        {
            SteerLimiter steerLimiter = carRoot.AddComponent<SteerLimiter>();
            steerLimiter.maxSteeringAngle = maxSteeringAngle;
            steerLimiter.minSteeringAngle = minSteeringAngle;
        }

        public static void AddAckerman(GameObject carRoot)
        {
            carRoot.AddComponent<AckerMan>();
        }

        /// <summary>
        /// Adds door functionality to your door
        /// </summary>
        /// <param name="door">The door object</param>
        /// <param name="minAngle">min HingeJoint angle</param>
        /// <param name="maxAngle">max HingeJoint angle</param>
        /// <param name="forceToAdd">The force to apply to the door (good value is 20)</param>
        /// <returns>The door component</returns>
        public static Door AddDoor(GameObject door, float minAngle, float maxAngle, float forceToAdd, Vector3 closedRotation)
        {
            HingeJoint hinge = door.GetComponent<HingeJoint>();
            if (hinge == null)
            {
                ModConsole.Log("[DrivableAPI] HingeJoint on door is null!");
                return null;
            }

            if (door.GetComponent<Rigidbody>() == null)
            {
                door.AddComponent<Rigidbody>();
            }

            Door doorComp = door.AddComponent<Door>();

            doorComp.hinge = hinge;
            doorComp.minAngle = minAngle;
            doorComp.maxAngle = maxAngle;
            doorComp.doorRB = door.GetComponent<Rigidbody>();
            doorComp.forceToAdd = forceToAdd;
            doorComp.doorClosedRotation = closedRotation;

            return doorComp;
        }
    }
}
