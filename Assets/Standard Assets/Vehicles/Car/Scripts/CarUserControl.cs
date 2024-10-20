using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using ArabicSupport;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    [RequireComponent(typeof (CarServer))]
   
    public class CarUserControl : MonoBehaviour
    {
        public bool isAtStartup = true;
        public bool isEditorMode = false;
        private CarController m_Car;
        private CarServer m_server;
        [Range(-100, 100)] [SerializeField] private float m_steer = 0;
        [Range(-10,10)] [SerializeField] private float m_accel = 0;
        [Range(-10, 10)] [SerializeField] private float m_handbrake = 0;

        public Scrollbar Steeringbar;
        public Scrollbar Speedbar;
        public Toggle Mode;
        public Text steeringText;
        public Text speedText;
        public Text logger;
    
        public int current_speed;
        public Text topSpeedText;
        public Slider topSpeedSlider;
        public int userTopSpeed = 30;
        public LanguageHandler languageHandler;
        private void Awake()
        {
            m_Car = GetComponent<CarController>();
            
            m_server = GetComponent<CarServer>();
            
            Steeringbar = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SteeringBar").GetComponentInChildren<Scrollbar>();
            Speedbar = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SpeedBar").GetComponentInChildren<Scrollbar>();
            Mode = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Toggles/ModeToggle").GetComponentInChildren<Toggle>();
            steeringText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SteeringText").GetComponentInChildren<Text>();
            speedText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/SpeedText").GetComponentInChildren<Text>();
            logger = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Log").GetComponentInChildren<Text>();
            topSpeedSlider = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Sliders/topSpeedSlider").GetComponentInChildren<Slider>();
            topSpeedText = GameObject.Find("UIPanel/Scroll View/Viewport/Content/Configuration/Sliders/TopSpeedText").GetComponentInChildren<Text>();
        }
        private void Start()
        {
            print("Engine Started ! CarUserControl.cs");
            topSpeedSlider.onValueChanged.AddListener (delegate {
                TopSpeedValueChangeCheck();
            });
            Mode.onValueChanged.AddListener(delegate {
                ToggleValueChanged(Mode);
            });

            GameObject gameObject = new GameObject("LanguageHandler");
            languageHandler = gameObject.AddComponent<LanguageHandler>();
            // languageHandler = new LanguageHandler();
		    languageHandler.m_dictionary();
            topSpeedText.text = "30" + languageHandler.dict["KPH"];
        
            if(languageHandler.lang == "fa"){
                Mode.GetComponentInChildren<Text>().text = ArabicFixer.Fix(languageHandler.dict["ManualControl"], false, false);
            }else{
                Mode.GetComponentInChildren<Text>().text = languageHandler.dict["ManualControl"];
            }
        }
        private void Update()
        {
            
        }
        void TopSpeedValueChangeCheck(){
            userTopSpeed = (int)topSpeedSlider.value;
            topSpeedText.text = userTopSpeed.ToString() + languageHandler.dict["KPH"];
        }
        void ToggleValueChanged(Toggle change)
        {
            if(Mode.isOn){
                isEditorMode = false;
            }else{
                isEditorMode = true;
            }
        }

        private void FixedUpdate()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            float handbrake = CrossPlatformInputManager.GetAxis("Forward");
            current_speed = Convert.ToInt32(GetComponent<Rigidbody>().velocity.magnitude*3.6);
            
            if (isEditorMode)
            {
                //Automatic Mode   
                m_steer = m_server.Steering;

                float scaledSteering = map(-100F, 100F, 0F, 1F, m_steer);
                Steeringbar.value = scaledSteering;
                steeringText.text = m_steer.ToString();
                float scaledSpeed = map(0F, (float)userTopSpeed, 0F, 1F, current_speed);
                Speedbar.size = scaledSpeed;
                speedText.text = (current_speed).ToString();

                float vSpeed = map(-100F, 100F, -1F, 1F, m_server.Speed);
                m_Car.m_Topspeed = userTopSpeed;
                if(vSpeed < 0){
                    m_Car.Move(m_steer / 100, 0.1F, 0.1F, m_handbrake);
                }
                m_Car.Move(m_steer / 100, vSpeed, vSpeed, m_handbrake);
            }
            else
            {
                //Manual Mode
                float scaledSteering = map(-1F, 1F, 0F, 1F, h);
                Steeringbar.value = scaledSteering;
                steeringText.text = ((int)(h*100)).ToString();

                float scaledSpeed = map(0F, (float)userTopSpeed, 0F, 1F, current_speed);
                Speedbar.size = scaledSpeed;
                speedText.text = (current_speed).ToString();

                m_Car.m_Topspeed = userTopSpeed;
                m_Car.Move(h, v, v, handbrake);
            }
        }
        public float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }
        
    }
}
