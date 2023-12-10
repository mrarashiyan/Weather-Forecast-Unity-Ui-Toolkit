using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UIComponents._General
{
    public class UiStartupInitializer : MonoBehaviour
    {
        [SerializeField] private BaseUISection m_StartupUi;
        [SerializeField] private UIDocument m_TargetUiDocument;

        private void Start()
        {
            m_StartupUi.Initialize(m_TargetUiDocument.rootVisualElement);
        }
    }
}