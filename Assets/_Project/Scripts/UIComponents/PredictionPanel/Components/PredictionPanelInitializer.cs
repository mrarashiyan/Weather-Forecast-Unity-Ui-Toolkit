using System.Collections;
using System.Collections.Generic;
using _Project.UI.HomePanel;
using _Project.UI.HomePanel.Modules;
using _Project.UI.PredictionPanel.Modules;
using _Project.UI.PredictionPanel.Modules.DataModels;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.PredictionPanel
{
    public class PredictionPanelInitializer : BaseUISection
    {
        private string WEATHER_CONTAINER_KEY = "Left";

        
        [SerializeField] private BaseUISection m_NextPanel;
        [SerializeField] private RootInitializer m_RootInitializer;
        [SerializeField] private SingleWeather[] m_WeatherData;
        [SerializeField] private CinemachineVirtualCamera m_CustomCamera;


        private List<SingleWeatherModule> m_LoadedModules = new List<SingleWeatherModule>();

        private void Update()
        {
            // check if Section is active
            if (GetSectionActivationStatus())
            {
                // wait for Scroll up or Up key pressed
                if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    // load the next Section and exit the current section
                    m_NextPanel.EnterSection();
                    ExitSection();
                }
            }
        }

        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            // clear cached modules if the this Section is load before
            m_LoadedModules.Clear();

            LoadModules();
        }

        public override void EnterSection()
        {
            base.EnterSection();
            
            // Change the main camera location (thanks by Cinemachine)
            m_CustomCamera.gameObject.SetActive(true);
            
            // Scroll the ScrollView in Root to show this section
            m_RootInitializer.ScrollTo(GetSectionContainer());
        }

        public override void ExitSection(bool autoUnloadModules = false)
        {
            base.ExitSection(true);
            
            // change main camera location to the original position
            m_CustomCamera.gameObject.SetActive(false);
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            InitializeWeatherData();
        }

        protected override void UnloadModules()
        {
            base.UnloadModules();

            foreach (var loadedModule in m_LoadedModules)
            {
                loadedModule.DeleteVisualElement();
            }
        }

        private void InitializeWeatherData()
        {
            // clear any previously added module in the Section
            var weatherRoot = GetSectionContainer().Q<VisualElement>(WEATHER_CONTAINER_KEY);
            for (int i = 0; i < weatherRoot.childCount; i++)
            {
                weatherRoot.RemoveAt(0);
            }
            
            // load data from WeatherData array to the WeatherModule
            foreach (var singleWeather in m_WeatherData)
            {
                var module = AddModule<SingleWeatherModule>();
                module.SetRoot(weatherRoot);
                module.SetData(singleWeather);
                module.LoadModule();
                module.InsertVisualElement();
                m_LoadedModules.Add(module);
            }
        }
    }
}