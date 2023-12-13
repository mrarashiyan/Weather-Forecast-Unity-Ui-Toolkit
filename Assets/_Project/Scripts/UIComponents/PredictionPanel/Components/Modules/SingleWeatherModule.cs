using System.Collections;
using System.Collections.Generic;
using _Project.UI.PredictionPanel.Modules.DataModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.PredictionPanel.Modules
{
    public class SingleWeatherModule : BaseUIModule
    {
        // path from the Resource folder
        private const string MODULE_PATH = "VisualTrees/SingleWeatherModule/SingleWeatherModule";
        
        // Selectors cons for finding Elements in UXML
        private const string ICON_HOLDER = "WeatherIcon";
        private const string WEATHER_TEMP = "WeatherTemp";
        private const string CITY_NAME = "CityName";
        
        private SingleWeather m_SingleWeather;
        
        public void SetData(SingleWeather singleWeather)
        {
            // assign Data Model to the module 
            m_SingleWeather = singleWeather;
        }
        
        
        public override void LoadModule()
        {
            SetModulePath(MODULE_PATH);
            base.LoadModule();

            var container = GetPreInsertContainer();
            container.Q<VisualElement>(ICON_HOLDER).style.backgroundImage =
                new StyleBackground(Background.FromSprite(m_SingleWeather.icon));

            container.Q<Label>(WEATHER_TEMP).text = m_SingleWeather.weatherTemp.ToString();
            container.Q<Label>(CITY_NAME).text = m_SingleWeather.cityName;
            
        }

    }
}