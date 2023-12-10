using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.HomePanel
{
    public class HomePanelInjector : BaseUIInjector
    {
        [ContextMenu("Initialize In Editor")]
        public override void Initialize()
        {
            base.Initialize();
        }

        [ContextMenu("Deinitialize in Editor")]
        public override void Deinitialize()
        {
            base.Deinitialize();
        }
    }
}