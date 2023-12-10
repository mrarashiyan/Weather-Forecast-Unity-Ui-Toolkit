using System;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.BaseComponents
{
    [ExecuteInEditMode]
    public class SectionEditorObject : MonoBehaviour
    {
        // we have this object to just check if any link is broken and we need to destroy it
        private GameObject m_ReferenceObject;
        private void OnEnable()
        {
            EditorApplication.delayCall +=  CheckReference;
        }

        private void OnDisable()
        {
            EditorApplication.delayCall -=  CheckReference;
        }

        private void OnDestroy()
        {
            EditorApplication.delayCall -=  CheckReference;
        }

        private void Start()
        {

            if(Application.isPlaying)
                DestroyObject();
        }

        private void DestroyObject()
        {
            DestroyImmediate(gameObject);
        }

        public void SetReference(GameObject reference)
        {
            m_ReferenceObject = reference;
        }

        private void CheckReference()
        {
            if(m_ReferenceObject == null)
                DestroyObject();
        }
    }
}