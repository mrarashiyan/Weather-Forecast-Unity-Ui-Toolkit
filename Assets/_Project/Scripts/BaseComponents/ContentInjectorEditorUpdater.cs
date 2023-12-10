using System;
using UnityEngine;

namespace _Project.Scripts.BaseComponents
{
    [ExecuteInEditMode]
    public class InjectorEditorUpdater : MonoBehaviour
    {
        [SerializeField] private bool m_LoadInEditor;

        private BaseUIInjector m_BaseUiInjector;
        private void OnEnable()
        {
            m_BaseUiInjector = GetComponent<BaseUIInjector>();
            
            if (Application.isPlaying == false)
            {
                UiDocumentPostprocessor.OnUiDocumentUpdated.RemoveListener(m_BaseUiInjector.UpdateEditor);
                if (m_LoadInEditor)
                {
                    UiDocumentPostprocessor.OnUiDocumentUpdated.AddListener(m_BaseUiInjector.UpdateEditor);
                    print(string.Format("[{0} : InjectorEditorUpdater] OnEnable : Registered in Runtime Ui Updater!", gameObject.name));
                }
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying == false)
            {
                UiDocumentPostprocessor.OnUiDocumentUpdated.RemoveListener(m_BaseUiInjector.UpdateEditor);
                //print(string.Format("[{0} : InjectorEditorUpdater] OnDisable : Unregistered in Runtime Ui Updater!", gameObject.name));
            }
        }
    }
}