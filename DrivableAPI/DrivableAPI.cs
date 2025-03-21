using MSCLoader;
using UnityEngine;

namespace DrivableAPI
{
    public class DrivableAPI : Mod
    {
        public override string ID => "DrivableAPI"; //Your mod ID (unique)
        public override string Name => "DrivableAPI"; //You mod name
        public override string Author => "0387"; //Your Username
        public override string Version => "1.0"; //Version
        public override string Description => ""; //Short description of your mod

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_Update);
        }

        internal static RaycastHit raycastHit;

        Camera cam;

        private void Mod_OnLoad()
        {
            cam = Camera.main;
            ModConsole.Log("Drivable API Loaded");
        }

        private void Mod_Update()
        {
            Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, 2f);
        }

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

            DriveTrigger.gameObject.layer = LayerMask.NameToLayer("PlayerOnlyColl");

            playerTrigger.gameObject.AddComponent<PlayerCarTrigger>();

            DrivingMode drivingMode = DriveTrigger.gameObject.AddComponent<DrivingMode>();
            drivingMode.offset = drivingModeOffset;
            drivingMode.AxisCarController = carRoot.GetComponent<AxisCarController>();
            drivingMode.PlayerPivotObject = DriveTrigger;
            drivingMode.carCustomName = carCustomName;

            return drivingMode;
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

        /// <summary>
        /// Adds door functionality to your door
        /// </summary>
        /// <param name="door">The door object</param>
        /// <param name="minAngle">min HingeJoint angle</param>
        /// <param name="maxAngle">max HingeJoint angle</param>
        /// <param name="forceToAdd">The force to apply to the door (good value is 20)</param>
        /// <returns>The door component</returns>
        public static Door AddDoor(GameObject door, float minAngle, float maxAngle, float forceToAdd)
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

            return doorComp;
        }
    }
}
