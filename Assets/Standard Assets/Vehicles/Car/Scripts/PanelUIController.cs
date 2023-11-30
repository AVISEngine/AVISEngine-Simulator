using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using ArabicSupport;

public class PanelUIController : MonoBehaviour
{
    public Button OpenPanel;
    public GameObject Panelcanvas;
    public GameObject Obstacles;
    public GameObject MainCanvas;
    // public GameObject DirectionalLight;
    
    private bool ismainPanel = true;
    Toggle obstacleToggle;
    Toggle rightSideToggle;
    Toggle headLightToggle;
    Canvas cgMain;
    Canvas cgPanel;
    Button reset;
    Button close;
    Button quit;
    Light worldLight;
    Slider dayTimeSlider;
    
    public GameObject RightCheckpoints;
    public GameObject FullCheckpoints;
    
    LanguageHandler languageHandler;
    Text StatusLabel;
    Text SteeringHeader;
    Text SpeedHeader;
    Text SensorsLabel;
    Text CreateServerLabel;
    Text LapsHeader;
    Text LogsHeader;
    Text ConfigHeader;

    Vector3 LightInitialTransform;
    Vector3 LightInitialPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Button to open the panel
        Button btn1 = OpenPanel.GetComponent<Button>();
        btn1.onClick.AddListener(OpenPanelEvent);

        //UI Panels : Logo Panel and Expanded Panel
        cgPanel = Panelcanvas.GetComponent<Canvas>();
        cgMain = MainCanvas.GetComponent<Canvas>();

        //Init Panel to show logo
        Show(cgMain);
        Hide(cgPanel);

        //Defining The UI Buttons
        reset = GameObject.Find("UIPanel/Reset").GetComponentInChildren<Button>();
        close = GameObject.Find("UIPanel/Close").GetComponentInChildren<Button>();
        quit = GameObject.Find("UIPanel/Levels").GetComponentInChildren<Button>();

        //Defining Toggles
        obstacleToggle = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Toggles/ObstaclesToggle").GetComponentInChildren<Toggle>();
        rightSideToggle = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Toggles/OneWayToggle").GetComponentInChildren<Toggle>();
        
        // DayLightSlider
        // dayTimeSlider = GameObject.Find("UIPanel/Scroll View/Viewport/Content/dayTime/dayTimeSlider")
        //     .GetComponentInChildren<Slider>();
        //DirectionalLight
        // worldLight = DirectionalLight.GetComponent<Light>();

        //Defining Texts
        StatusLabel = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Status Label").GetComponentInChildren<Text>();
        SteeringHeader = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SteeringHeader").GetComponentInChildren<Text>();
        SpeedHeader = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SpeedHeader").GetComponentInChildren<Text>();
        SensorsLabel = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Sensors Label").GetComponentInChildren<Text>();
        CreateServerLabel = GameObject.Find("UIPanel/Scroll View/Viewport/Content/CreateServer Label").GetComponentInChildren<Text>();
        LapsHeader = GameObject.Find("UIPanel/Scroll View/Viewport/Content/LapsAndCheckpointsLabel").GetComponentInChildren<Text>();
        LogsHeader = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Logs Label").GetComponentInChildren<Text>();
        ConfigHeader = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Config Label").GetComponentInChildren<Text>();

        //Language Handler
        GameObject gameObject = new GameObject("LanguageHandler");
        languageHandler = gameObject.AddComponent<LanguageHandler>();
        
        // LightInitialTransform = new Vector3(worldLight.transform.localRotation.eulerAngles.x, worldLight.transform.localRotation.eulerAngles.y ,worldLight.transform.localRotation.eulerAngles.z);
        // LightInitialPosition = new Vector3(worldLight.transform.position.x,
        //     worldLight.transform.position.y, worldLight.transform.position.z);
        
        languageHandler.m_dictionary();
        if(languageHandler.lang == "fa"){
            btn1.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["InfoPanel"], false, false);
            reset.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Reset"], false, false);
            quit.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["QuitToMenu"], false, false);
            close.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["CloseThisPanel"], false, false);
            StatusLabel.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Status"], false, false);
            SteeringHeader.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["SteeringAngle"], false, false);
            SpeedHeader.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Speed"], false, false);
            SensorsLabel.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Sensors"], false, false);
            CreateServerLabel.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["StartServer"], false, false);
            LapsHeader.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["LapsAndCheckpoints"], false, false);
            LogsHeader.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Logs"], false, false);
            ConfigHeader.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Configuration"], false, false);
            obstacleToggle.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["Obstacles"], false, false);
            rightSideToggle.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["RightLaneCheckpoint"], false, false);
        }else{
            btn1.GetComponentInChildren<Text>().text = languageHandler.dict["InfoPanel"];
            reset.GetComponentInChildren<Text>().text = languageHandler.dict["Reset"];
            quit.GetComponentInChildren<Text>().text = languageHandler.dict["QuitToMenu"];
            close.GetComponentInChildren<Text>().text = languageHandler.dict["CloseThisPanel"];
            StatusLabel.GetComponentInChildren<Text>().text = languageHandler.dict["Status"];
            SteeringHeader.GetComponentInChildren<Text>().text = languageHandler.dict["SteeringAngle"];
            SpeedHeader.GetComponentInChildren<Text>().text = languageHandler.dict["Speed"];
            SensorsLabel.GetComponentInChildren<Text>().text = languageHandler.dict["Sensors"];
            CreateServerLabel.GetComponentInChildren<Text>().text = languageHandler.dict["StartServer"];
            LapsHeader.GetComponentInChildren<Text>().text = languageHandler.dict["LapsAndCheckpoints"];
            LogsHeader.GetComponentInChildren<Text>().text = languageHandler.dict["Logs"];
            ConfigHeader.GetComponentInChildren<Text>().text = languageHandler.dict["Configuration"];
            obstacleToggle.GetComponentInChildren<Text>().text = languageHandler.dict["Obstacles"];
            rightSideToggle.GetComponentInChildren<Text>().text = languageHandler.dict["RightLaneCheckpoint"];
        }

        //Adding listeners to buttons
        reset.onClick.AddListener(resetEvent);
        quit.onClick.AddListener(quitEvent);
        close.onClick.AddListener(closeEvent);

        //Set Default Values
        RightCheckpoints.SetActive(true);
        FullCheckpoints.SetActive(false);

        
        //Defining Checkpoint GameObjects
        // RightCheckpoints = GameObject.Find("RightCheckPoints").GetComponent();
        // FullCheckpoints = GameObject.Find("FullCheckPoints").GetComponent();
    }

    void quitEvent()
    {
        SceneManager.LoadScene("NewMainMenu");
    }
    void resetEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void closeEvent()
    {
        Show(cgMain);
        Hide(cgPanel);
        ismainPanel = true;
    }
    void dayTimeValueChanged(Slider slider)
    {
    
        Quaternion lightTransform =  Quaternion.Euler(slider.value, LightInitialTransform.y ,LightInitialTransform.y);
        // print(lightTransform.ToString());
        //
        // worldLight.transform.SetPositionAndRotation(LightInitialPosition, lightTransform);
    }

    // Update is called once per frame
    void Update()
    {
        rightSideToggle.onValueChanged.AddListener(delegate{
            WayToggleValueChanged(rightSideToggle);
        });
        obstacleToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(obstacleToggle);
            
        });
        
        // dayTimeSlider.onValueChanged.AddListener(delegate {
        //     dayTimeValueChanged(dayTimeSlider);
        //     
        // });
    }
    void WayToggleValueChanged(Toggle change){
        if(change.isOn){
            RightCheckpoints.SetActive(true);
            FullCheckpoints.SetActive(false);
        }else{
            RightCheckpoints.SetActive(false);
            FullCheckpoints.SetActive(true);
        }
    }
    void ToggleValueChanged(Toggle change)
    {
        if (obstacleToggle.isOn)
        {
            Obstacles.SetActive(true);
        }
        else
        {
            Obstacles.SetActive(false);
        }
        //print(obstacleToggle.isOn);
    }
    void OpenPanelEvent()
    {   
        print("Hey") ;
        if (ismainPanel)
        {
    
            //MainCanvas.SetActive(false);
            //Panelcanvas.SetActive(true);
            Hide(cgMain);
            Show(cgPanel);
            ismainPanel = false;
        }
        else
        {
            //MainCanvas.SetActive(true);
            //Panelcanvas.SetActive(false);
            Show(cgMain);
            Hide(cgPanel);
            ismainPanel = true;
        }
        
    }

    void OnGUI()
    {
        GUIStyle localStyle = new GUIStyle(GUI.skin.label);
        localStyle.normal.textColor = Color.black;
        if (ismainPanel)
        {
            // GUI.Label(new Rect(10, 200, 300, 100), "Press S to hide the info panel", localStyle);
            ismainPanel = false;
        }
        else
        {
          
            ismainPanel = true;
            
        }

    }


    void Hide(Canvas canvas)
    {
        canvas.enabled = false;
     
    }
    void Show(Canvas canvas)
    {
        canvas.enabled = true;
    }
}
