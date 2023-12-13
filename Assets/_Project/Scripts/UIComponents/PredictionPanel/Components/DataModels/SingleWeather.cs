using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.UI.PredictionPanel.Modules.DataModels
{
    [Serializable]
    public class SingleWeather
    {
        public Sprite icon;
        public string cityName;
        public int weatherTemp;
    }
}