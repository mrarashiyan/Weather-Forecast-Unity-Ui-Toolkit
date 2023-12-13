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

        [Header("News Container")]
        [SerializeField] private string m_NewsScrollviewKey = "NewsContainer";
        [SerializeField] private SingleNews[] m_NewsArray;
        [SerializeField] private float m_NewsHoldDuration = 8;

        [Header("Trackers Container")]
        [SerializeField] private Transform m_CutPlane;
        [SerializeField] private Camera m_MainCamera;
        [SerializeField] private Transform[] m_InGameTrackers;

        [Header("Transition to next Container")]
        [SerializeField] private RootInitializer m_RootInitializer;
        [SerializeField] private BaseUISection m_NextPanel;

        // elements in window        
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
            if (GetSectionActivationStatus())
            {
                if (m_PlayerRadialProgressbar != null)
                {
                    m_CircleTimerValue += Time.deltaTime * m_NewsHoldDuration * 2;
                    m_PlayerRadialProgressbar.progress = m_CircleTimerValue;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_NextPanel.Initialize(m_Root);
                    m_NextPanel.EnterSection();
                    ExitSection();
                }
            }
        }

        public override void EnterSection()
        {
            base.EnterSection();
            
            m_RootInitializer.ScrollTo(GetSectionContainer());
        }

        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            m_PlayerRadialProgressbar = GetSectionContainer().Q<RadialProgress>(className: PLAY_SCROLL_CLASS);
            m_MouseScrollIcon = GetSectionContainer().Q<VisualElement>(classes: MOUSE_ICON);

            LoadModules();
            Invoke(nameof(AnimateMouseScroll), 1);
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            InitializeNews(m_Root);
            DoSlideNews();
            
            InitializeCountryPins(m_Root);
        }


        #region News Module

        private void InitializeNews(VisualElement root)
        {
            var newsContainer = root.Q<ScrollView>(m_NewsScrollviewKey);
            newsContainer.RemoveAt(0);

            foreach (var singleNews in m_NewsArray)
            {
                var singleNewsModule = AddModule<SingleNewsModule>();
                singleNewsModule.SetRoot(newsContainer);
                singleNewsModule.SetData(singleNews);
                singleNewsModule.LoadModule();
                singleNewsModule.GetPreInsertContainer().AddToClassList("SingleNews--Hide");
                singleNewsModule.InsertVisualElement();
                m_LoadedNewsModules.Add(singleNewsModule);
            }
        }

        private void DoSlideNews()
        {
            foreach (var singleNews in m_LoadedNewsModules)
            {
                singleNews.GetInsertedContainer().AddToClassList("SingleNews--Hide");
            }

            m_LoadedNewsModules[m_CurrentLoadedNewsIndex].GetInsertedContainer()
                .RemoveFromClassList("SingleNews--Hide");
            print("[HomePanelInitializer] DoSlideNews: Show News Id=" +
                  m_LoadedNewsModules[m_CurrentLoadedNewsIndex].GetModuleId());

            m_CurrentLoadedNewsIndex++;
            if (m_CurrentLoadedNewsIndex >= m_LoadedNewsModules.Count)
            {
                m_CurrentLoadedNewsIndex = 0;
            }

            m_CircleTimerValue = 0;

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
                trackerModule.SetTransforms(inGameTracker,m_MainCamera,m_CutPlane);
                trackerModule.LoadModule();
                trackerModule.InsertVisualElement();
                m_LoadedCountryPinModules.Add(trackerModule);
            }
        }

        #endregion
        
        
        private void AnimateMouseScroll()
        {
            m_MouseScrollIcon.RegisterCallback<TransitionEndEvent>(evnt =>
                m_MouseScrollIcon.ToggleInClassList("MouseScroll--Up"));
            
            m_MouseScrollIcon.ToggleInClassList("MouseScroll--Up");

        }
    }
}