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
        // path from the Resource folder
        private const string MODULE_PATH = "VisualTrees/CountryPin/CountryPin";
        
        // Selectors cons for finding Elements in UXML
        private const string WAVE_ELEMENT = "CircleCenterWave";

        private VisualElement m_WaveElement;
        private Transform m_TrackingTransform;
        private Camera m_TrackingCamera;
        private Transform m_CutPlane;

        /// <summary>
        /// Set data that we need to convert a WorldSpace transform to a Vector2 ui position
        /// </summary>
        /// <param name="trackingTransform"></param>
        /// <param name="mainCamera"></param>
        /// <param name="cutPlane">points behind of this plane will be hidden from the screen</param>
        public void SetTransforms(Transform trackingTransform, Camera mainCamera, Transform cutPlane)
        {
            m_TrackingCamera = mainCamera;
            m_TrackingTransform = trackingTransform;
            m_CutPlane = cutPlane;
        }
        
        public override void LoadModule()
        {
            // Set Module Path and Load the VisualTreeAsset
            SetModulePath(MODULE_PATH);
            base.LoadModule();

            // get the Wave element inside of the pin
            m_WaveElement = GetPreInsertContainer().Q<VisualElement>(className: WAVE_ELEMENT);
            
            // Wait for the drawing of the first frame and then Run the animation of bottom Mouse icon
            Invoke(nameof(AnimateWave),0.5f);
        }

        private void Update()
        {
            if (m_WaveElement != null && m_TrackingTransform)
            {
                // check if the object that we are tracking is not behind the Cut plane
                if (m_TrackingTransform.position.x < m_CutPlane.position.x)
                {
                    // if not, remove Hide class and start to move the pin to TrackingTransform position
                    GetInsertedContainer().RemoveFromClassList("CountryPin--Hide");
                    AlignWithTracker();
                }
                else
                {
                    //if TrackingTransform is behind the plane, just Hide it 
                    GetInsertedContainer().AddToClassList("CountryPin--Hide");
                }
            }
        }

        private void AlignWithTracker()
        {
            // Convert World position of TrackingTransform to UI position of current Section
            // we use this function to relate different UI Resolution and Screen Resolution 
            var screenPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
                m_Root.panel,m_TrackingTransform.position,m_TrackingCamera
            );
            
            // update the pin position in style
            GetInsertedContainer().style.left = screenPosition.x;
            GetInsertedContainer().style.top = screenPosition.y;
        }

        
        private void AnimateWave()
        {
            m_WaveElement.AddToClassList("CircleCenterWave--Large");

            // Pulsing animation contains of 4 different parameter, and
            // TransitionEndEvent is called for each transition. So here we are making sure
            // that all of the animations are finished successfully and then run our lines
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
        
    }
}