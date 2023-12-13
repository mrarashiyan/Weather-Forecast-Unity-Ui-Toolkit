using System.Collections;
using System.Collections.Generic;
using _Project.UI.PredictionPanel.Modules.DataModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.PredictionPanel.Modules
{
    public class SingleWeatherModule : BaseUIModule
    {
        private const string MODULE_PATH = "VisualTrees/SingleWeatherModule/SingleWeatherModule";
        
        private const string ICON_HOLDER = "WeatherIcon";
        private const string WEATHER_TEMP = "WeatherTemp";
        private const string CITY_NAME = "CityName";
        
        private SingleWeather m_SingleWeather;


        public void SetWeatherData(SingleWeather singleWeather)
        {
            m_SingleWeather = singleWeather;
        }

        public void SetData(SingleWeather singleWeather)
        {
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