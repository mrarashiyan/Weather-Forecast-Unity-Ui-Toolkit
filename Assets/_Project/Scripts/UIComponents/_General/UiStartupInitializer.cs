using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.UIComponents._General
{
    public class UiStartupInitializer : MonoBehaviour
    {
        [SerializeField] private BaseUISection m_StartupUi;
        [SerializeField] private UIDocument m_TargetUiDocument;

        IEnumerator Start()
        {
            m_StartupUi.Initialize(m_TargetUiDocument.rootVisualElement);
            yield return new WaitForEndOfFrame();
            m_StartupUi.EnterSection();
        }
    }
}