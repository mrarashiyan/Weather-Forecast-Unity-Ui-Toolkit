using System;
using UnityEngine;

namespace _Project.UI.HomePanel.Modules.DataModels
{
    [Serializable]
    public class SingleNews 
    {
        public Texture2D thumbnail;
        public string title;
        public string desc;
        public string tags;
        public string link;
    }
}