using UnityEngine;

namespace DrivableAPI
{
    internal class GearIndicator : MonoBehaviour
    {
        TextMesh[] GUIGearText = new TextMesh[2];
        private Drivetrain drivetrain;

        public string carCustomName;
        public bool automatic;

        void Start()
        {
            GUIGearText[0] = GameObject.Instantiate(GameObject.Find("GUI/Indicators/Gear")).GetComponent<TextMesh>();
            GUIGearText[0].GetComponent<PlayMakerFSM>().enabled = false;
            GUIGearText[0].transform.parent = GameObject.Find("GUI/Indicators").transform;
            GUIGearText[0].transform.localPosition = new Vector3(12, 0, 0);
            GUIGearText[0].gameObject.name = carCustomName + "GearIndicator";
            GUIGearText[1] = GUIGearText[0].transform.GetChild(0).GetComponent<TextMesh>();

            drivetrain = GetComponent<Drivetrain>();
        }

        void Update()
        {
            if (PlayMakerGlobals.Instance.Variables.FindFsmString("PlayerCurrentVehicle").Value == carCustomName)
            {
                if (!GUIGearText[0].gameObject.activeSelf)
                {
                    GUIGearText[0].gameObject.SetActive(true);
                    GUIGearText[0].GetComponent<MeshRenderer>().enabled = true;
                }

                if (drivetrain.gear == 0)
                {
                    GUIGearText[0].text = "R";
                    GUIGearText[1].text = "R";
                }
                else if (drivetrain.gear == 1)
                {
                    GUIGearText[0].text = "N";
                    GUIGearText[1].text = "N";
                }
                else if (drivetrain.gear > 1 && !automatic)
                {
                    GUIGearText[0].text = $"{drivetrain.gear - 1}";
                    GUIGearText[1].text = $"{drivetrain.gear - 1}";
                }
                else if (drivetrain.gear > 1)
                {
                    GUIGearText[0].text = "D";
                    GUIGearText[1].text = "D";
                }
            }
            else
            {
                if (GUIGearText[0].gameObject.activeSelf)
                {
                    GUIGearText[0].gameObject.SetActive(false);
                    GUIGearText[0].GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }
}
