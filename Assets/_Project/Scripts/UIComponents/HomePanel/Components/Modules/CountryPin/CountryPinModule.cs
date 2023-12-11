using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.HomePanel.Modules
{
    public class CountryPinModule : BaseUIModule
    {
        private const string MODULE_PATH = "VisualTrees/CountryPin/CountryPin";
        private const string WAVE_ELEMENT = "CircleCenterWave";

        private VisualElement m_WaveElement;
        private Transform m_TrackingTransform;
        private Camera m_TrackingCamera;
        private Transform m_CutPlane;

        
        public void SetTransforms(Transform trackingTransform, Camera mainCamera, Transform cutPlane)
        {
            m_TrackingCamera = mainCamera;
            m_TrackingTransform = trackingTransform;
            m_CutPlane = cutPlane;
        }
        
        public override void LoadModule()
        {
            SetModulePath(MODULE_PATH);
            base.LoadModule();

            m_PreInsertContainer = m_VisualAsset.Instantiate().ElementAt(0);
            
            foreach (var styleSheet in m_VisualAsset.stylesheets)
            {
                m_PreInsertContainer.styleSheets.Add(styleSheet);
            }
            
            m_WaveElement = m_PreInsertContainer.Q<VisualElement>(className: WAVE_ELEMENT);
            Invoke(nameof(AnimateWave),0.5f);
        }

        private void Update()
        {
            if (m_WaveElement != null && m_TrackingTransform  && CanSee())
            {
                if (m_TrackingTransform.position.x < m_CutPlane.position.x)
                {
                    m_PreInsertContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    AlignWithTracker();
                }
                else
                {
                    m_PreInsertContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                }
            }
        }

        private void AlignWithTracker()
        {
            var screenPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
                m_Root.panel,m_TrackingTransform.position,m_TrackingCamera
            );
            //var screenPosition = m_TrackingCamera.WorldToScreenPoint(m_TrackingTransform.position);
            GetInsertedContainer().style.left = screenPosition.x;
            GetInsertedContainer().style.top = screenPosition.y;
        }

        
        private void AnimateWave()
        {
            m_WaveElement.AddToClassList("CircleCenterWave--Large");

            int endEventRepeatedCount = 0;
            m_WaveElement.RegisterCallback<TransitionEndEvent>(evt =>
            {
                endEventRepeatedCount++;
                if (m_WaveElement.resolvedStyle.transitionProperty.Count() == endEventRepeatedCount)
                {
                    m_WaveElement.ToggleInClassList("CircleCenterWave--Large");
                    endEventRepeatedCount = 0;
                }
            });
        }

        private bool CanSee()
        {
            return true;
        }
    }
}