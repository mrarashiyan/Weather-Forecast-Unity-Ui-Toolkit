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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ExitSection();
                m_NextPanel.EnterSection();
            }
        }

        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            m_LoadedModules.Clear();

            LoadModules();
        }

        public override void EnterSection()
        {
            base.EnterSection();
            m_CustomCamera.gameObject.SetActive(true);
            m_RootInitializer.ScrollTo(GetSectionContainer());
        }

        public override void ExitSection(bool autoUnloadModules = false)
        {
            base.ExitSection(true);
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
            var weatherRoot = GetSectionContainer().Q<VisualElement>(WEATHER_CONTAINER_KEY);
            for (int i = 0; i < weatherRoot.childCount; i++)
            {
                weatherRoot.RemoveAt(0);
            }
            
            foreach (var singleWeather in m_WeatherData)
            {
                var module = AddModule<SingleWeatherModule>();
                module.SetRoot(weatherRoot);
                module.SetData(singleWeather);
                module.LoadModule();
                module.GetPreInsertContainer().AddToClassList("SingleWeather--Hide");
                module.InsertVisualElement();
                m_LoadedModules.Add(module);
            }
        }
    }
}