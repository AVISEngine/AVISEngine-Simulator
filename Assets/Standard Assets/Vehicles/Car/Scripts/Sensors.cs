//RayCast

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;
public class Sensors : MonoBehaviour
{   
    public float distanceCenter;
    public float distanceRight;
    public float distanceLeft;
    Text CenterText;
    Text RightText;
    Text LeftText;
    Text AngleText;
    Text TopSpeedHeaderText;
    Text AngleHeaderText;
    Text CheckpointsHeaderText;
    public Scrollbar centerbar;
    public Scrollbar rightbar;
    public Scrollbar leftbar;
    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;
    public Slider sensorAngleSlider;
    public Toggle sensorLinesToggle;

    private LineRenderer sensorsLine;
    public int[] maxNoise = {0, 50};
    bool useSphereCast;
    public LanguageHandler languageHandler;

    Font iranSans;

    void Start()
    {
        //Language Handler
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
		languageHandler.m_dictionary();
        iranSans = Resources.Load("IRANSans") as Font;

        distanceCenter = 0; //Distance initial value set to 0
        distanceRight = 0; //Distance initial value set to 0
        distanceLeft = 0; //Distance initial value set to 0
        
        sensorsLine = GetComponent<LineRenderer> ();
        sensorLinesToggle = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Toggles/VisibleRay Toggle").GetComponentInChildren<Toggle>();
        
        CenterText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/CenterSensor").GetComponentInChildren<Text>();
        RightText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/RightSensor").GetComponentInChildren<Text>();
        LeftText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/LeftSensor").GetComponentInChildren<Text>();

        centerbar = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SensorBarCenter").GetComponentInChildren<Scrollbar>();
        rightbar = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SensorBarRight").GetComponentInChildren<Scrollbar>();
        leftbar = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SensorBarLeft").GetComponentInChildren<Scrollbar>();

        AngleText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Sliders/AngleText").GetComponentInChildren<Text>();
        sensorAngleSlider = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Sliders/castAngleSlider").GetComponentInChildren<Slider>();
        TopSpeedHeaderText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Sliders/Text").GetComponent<Text>();
        AngleHeaderText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Sliders/Text (1)").GetComponent<Text>();
        CheckpointsHeaderText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/LapsAndCheckpointsLabel").GetComponent<Text>();

        CenterText.text = "-";
        RightText.text = "-";
        LeftText.text = "-";
        if(languageHandler.lang == "fa"){
            AngleText.text = "30" + ArabicFixer.Fix(languageHandler.dict["Degrees"], false, false);
            TopSpeedHeaderText.text = ArabicFixer.Fix(languageHandler.dict["TopSpeed"], false, false);
            AngleHeaderText.font = iranSans;
            AngleHeaderText.text = ArabicFixer.Fix(languageHandler.dict["SensorAngle"], false, false);
            CheckpointsHeaderText.font = iranSans;
            CheckpointsHeaderText.text = ArabicFixer.Fix(languageHandler.dict["LapsAndCheckpointsHeader"], false, false);
            sensorLinesToggle.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["VisibleSensorRay"], false, false);
        }else{
            AngleText.text = "30" + languageHandler.dict["Degrees"];
            TopSpeedHeaderText.text = languageHandler.dict["TopSpeed"];
            AngleHeaderText.text = languageHandler.dict["SensorAngle"];
            CheckpointsHeaderText.text = languageHandler.dict["LapsAndCheckpointsHeader"];
            sensorLinesToggle.GetComponentInChildren<Text>().text = languageHandler.dict["VisibleSensorRay"];
        }
        sensorAngleSlider.onValueChanged.AddListener(delegate {
            SliderValueChanged();
        });

        sensorLinesToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(sensorLinesToggle);
        });
    }
    // Update is called once per frame
    void Update()
    {
        sensor();
    }
    void SliderValueChanged()
    {
        frontSensorAngle = sensorAngleSlider.value;
        if(languageHandler.lang == "fa"){
            AngleText.text = frontSensorAngle.ToString() + ArabicFixer.Fix(languageHandler.dict["Degrees"], false, false);
        }else{
            AngleText.text = frontSensorAngle.ToString() + languageHandler.dict["Degrees"];
        }
    }
    void ToggleValueChanged(Toggle change)
    {
        if(sensorLinesToggle.isOn){
            sensorsLine.enabled = true;
        }else{
            sensorsLine.enabled = false;
        }
    }

    private void sensor()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        
        //center sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        sensorsLine.SetPosition(0,sensorStartPos);
        sensorsLine.SetPosition(1,(transform.forward * 15) + sensorStartPos);

        
        // if (Physics.SphereCast(sensorStartPos, 1, transform.forward, out hit, sensorLength))
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                
                // Gizmos.DrawWireSphere(sensorStartPos + transform.forward * hit.distance, 1);
                distanceCenter = hit.distance;
                if((distanceCenter * 100F) > 100){
                    int noise = RandomNumber(maxNoise[0],maxNoise[1]);
                    distanceCenter = distanceCenter - (noise/100F);
                }

                CenterText.text = ((int)(distanceCenter * 100f)).ToString() + languageHandler.dict["cm"];
                float scaledValue = map(0F, 1500F, 1F, 0F, (distanceCenter * 100f));
                centerbar.size = scaledValue;

            }
            else
            {
                distanceCenter = 15;
                CenterText.text = "+1500" + languageHandler.dict["cm"];
                centerbar.size = 0;
            }
        }
        else
        {
            distanceCenter = 15;
            CenterText.text = "+1500" + languageHandler.dict["cm"];
            centerbar.size = 0;
        }

        //front right angle sensor
        sensorsLine.SetPosition(2,sensorStartPos);
        sensorsLine.SetPosition(3,(Quaternion.AngleAxis(frontSensorAngle, transform.up) * (transform.forward * 15)) + sensorStartPos);
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                distanceRight = hit.distance;
                if((distanceRight * 100F) > 100){
                    int noise = RandomNumber(maxNoise[0],maxNoise[1]);
                    distanceRight = distanceRight - (noise/100F);
                }

                RightText.text = ((int)(distanceRight * 100f)).ToString() + languageHandler.dict["cm"];
                float scaledValue = map(0F, 1500F, 1F, 0F, (distanceRight * 100f));
                rightbar.size = scaledValue;
            }
            else
            {
                distanceRight = 15;
                RightText.text = "+1500" + languageHandler.dict["cm"];
                rightbar.size = 0;
            }
        }
        else
        {
            distanceRight = 15;
            RightText.text = "+1500" + languageHandler.dict["cm"];
            rightbar.size = 0;
        }


        sensorsLine.SetPosition(4,sensorStartPos);
        sensorsLine.SetPosition(5,(Quaternion.AngleAxis(-frontSensorAngle, transform.up) * (transform.forward * 15)) + sensorStartPos);
        //front left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                distanceLeft = hit.distance;
                if((distanceLeft * 100F) > 100){
                    int noise = RandomNumber(maxNoise[0],maxNoise[1]);
                    distanceLeft = distanceLeft - (noise/100F);
                }
                LeftText.text = ((int)(distanceLeft * 100f)).ToString() + languageHandler.dict["cm"];
                float scaledValue = map(0F, 1500F, 1F, 0F, (distanceLeft * 100f));
                leftbar.size = scaledValue;

            }
            else
            {
                distanceLeft = 15;
                LeftText.text = "+1500" + languageHandler.dict["cm"];
                leftbar.size = 0;
            }
        }
        else
        {
            distanceLeft = 15;
            LeftText.text = "+1500" + languageHandler.dict["cm"];
            leftbar.size = 0;
        }
        // #region FRONTLEFT 
        // //front left sensor
        // sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        // if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        // {
        //     if (!hit.collider.CompareTag("Terrain"))
        //     {
        //         Debug.DrawLine(sensorStartPos, hit.point);

        //     }
        // }
        // #endregion

    }
    public float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    public int RandomNumber(int min, int max)  
    {  
        System.Random random = new System.Random();  
        return random.Next(min, max);  
    }  
    
}
