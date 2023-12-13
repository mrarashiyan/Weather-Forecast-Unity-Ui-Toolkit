using System;
using System.Collections;
using System.Collections.Generic;
using _Project.UI.HomePanel.Modules;
using UnityEngine;
using UnityEngine.UIElements;
using _Project.UI.HomePanel.Modules.DataModels;

namespace _Project.UI.HomePanel
{
    public class HomePanelInitializer : BaseUISection
    {
        private const string PLAY_SCROLL_CLASS = "CircleTimer";
        private const string MOUSE_ICON = "MouseScroll";

        [Header("News Container")] [SerializeField]
        private string m_NewsScrollviewKey = "NewsContainer";

        [SerializeField] private SingleNews[] m_NewsArray;
        [SerializeField] private float m_NewsHoldDuration = 8;

        [Header("Trackers Container")] [SerializeField]
        private Transform m_CutPlane;

        [SerializeField] private Camera m_MainCamera;
        [SerializeField] private Transform[] m_InGameTrackers;

        [Header("Transition to next Container")] [SerializeField]
        private RootInitializer m_RootInitializer;

        [SerializeField] private BaseUISection m_NextPanel;

        // Elements in window        
        private VisualElement m_MouseScrollIcon;
        private RadialProgress m_PlayerRadialProgressbar;

        // News variables
        private int m_CurrentLoadedNewsIndex = 0;
        private List<SingleNewsModule> m_LoadedNewsModules = new List<SingleNewsModule>();
        private float m_CircleTimerValue = 0;

        // Trackers Variables
        private List<CountryPinModule> m_LoadedCountryPinModules = new List<CountryPinModule>();

        private void Update()
        {
            // Check if current Section is showing to the player
            if (GetSectionActivationStatus())
            {
                // if Radial Progressbar exists on the Section, change it's value
                if (m_PlayerRadialProgressbar != null)
                {
                    // fill Progressbar with a certain speed during NewsHoldDuration 
                    m_CircleTimerValue += Time.deltaTime * m_NewsHoldDuration * 2;
                    m_PlayerRadialProgressbar.progress = m_CircleTimerValue;
                }

                // if scroll down or press down is pressed, start the transition
                if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    // make ready the next Section and switch to it
                    m_NextPanel.Initialize(m_Root);
                    m_NextPanel.EnterSection();

                    // then exit and deactivate current section
                    ExitSection();
                }
            }
        }

        public override void EnterSection()
        {
            base.EnterSection();

            // When this section is getting activated, scroll down or up the ScrollView to show current Section
            m_RootInitializer.ScrollTo(GetSectionContainer());
        }

        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            // initialize variables
            m_PlayerRadialProgressbar = GetSectionContainer().Q<RadialProgress>(className: PLAY_SCROLL_CLASS);
            m_MouseScrollIcon = GetSectionContainer().Q<VisualElement>(classes: MOUSE_ICON);

            // Load all of modules like News or Country Pins
            LoadModules();

            // Wait for the drawing of the first frame and then Run the animation of bottom Mouse icon
            Invoke(nameof(AnimateMouseScroll), 1);
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            //Initialize the News module and start sliding effect between them
            InitializeNews(m_Root);
            DoSlideNews();

            // initialize pin and wave effect on the countries
            InitializeCountryPins(m_Root);
        }


        #region News Module

        private void InitializeNews(VisualElement root)
        {
            // remove any previously added element
            var newsContainer = root.Q<ScrollView>(m_NewsScrollviewKey);
            newsContainer.RemoveAt(0);

            // read NewsArray and convert the data to the Modules
            foreach (var singleNews in m_NewsArray)
            {
                var singleNewsModule = AddModule<SingleNewsModule>();

                singleNewsModule.SetRoot(newsContainer);
                singleNewsModule.SetData(singleNews);
                singleNewsModule.LoadModule();

                // hide them at the first
                singleNewsModule.GetPreInsertContainer().AddToClassList("SingleNews--Hide");

                singleNewsModule.InsertVisualElement();

                // cache them in a List for future access
                m_LoadedNewsModules.Add(singleNewsModule);
            }
        }

        private void DoSlideNews()
        {
            // in the first step hide all of NewsModule in the News Section
            foreach (var singleNews in m_LoadedNewsModules)
            {
                singleNews.GetInsertedContainer().AddToClassList("SingleNews--Hide");
            }

            // then get the next NewsModule and show it in the section
            m_LoadedNewsModules[m_CurrentLoadedNewsIndex].GetInsertedContainer()
                .RemoveFromClassList("SingleNews--Hide");

            print("[HomePanelInitializer] DoSlideNews: Show News Id=" +
                  m_LoadedNewsModules[m_CurrentLoadedNewsIndex].GetModuleId());

            // increase the counter
            m_CurrentLoadedNewsIndex++;
            if (m_CurrentLoadedNewsIndex >= m_LoadedNewsModules.Count)
            {
                m_CurrentLoadedNewsIndex = 0;
            }

            // reset the slider timer
            m_CircleTimerValue = 0;

            // call this function in X seconds later
            Invoke(nameof(DoSlideNews), m_NewsHoldDuration);
        }

        #endregion

        #region CountryPins Module

        private void InitializeCountryPins(VisualElement root)
        {
            foreach (var inGameTracker in m_InGameTrackers)
            {
                var trackerModule = AddModule<CountryPinModule>();
                trackerModule.SetRoot(root);
                
                // set the following transform and the target camera for each pin
                trackerModule.SetTransforms(inGameTracker, m_MainCamera, m_CutPlane);
                
                trackerModule.LoadModule();
                trackerModule.InsertVisualElement();
                m_LoadedCountryPinModules.Add(trackerModule);
            }
        }

        #endregion


        private void AnimateMouseScroll()
        {
            // start animating to up
            m_MouseScrollIcon.ToggleInClassList("MouseScroll--Up");

            // when the animation is finished, remove it (icon will return to the original position)
            // again when returning animation is done, this function will be called again and add Up animation to the element
            m_MouseScrollIcon.RegisterCallback<TransitionEndEvent>(evnt =>
                m_MouseScrollIcon.ToggleInClassList("MouseScroll--Up"));
        }
    }
}