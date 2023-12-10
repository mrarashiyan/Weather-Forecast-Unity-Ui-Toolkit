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


        [SerializeField] private string m_NewsScrollviewKey = "NewsContainer";
        [SerializeField] private SingleNews[] m_NewsArray;
        [SerializeField] private float m_NewsHoldDuration = 8;


        private int m_CurrentLoadedNewsIndex = 0;
        private List<SingleNewsModule> m_LoadedNewsModules = new List<SingleNewsModule>();
        private float m_CircleTimerValue = 0;
        private RadialProgress m_PlayerRadialProgressbar;
        private VisualElement m_MouseScrollIcon;

        private void Update()
        {
            if (m_PlayerRadialProgressbar != null)
            {
                m_CircleTimerValue += Time.deltaTime * m_NewsHoldDuration * 2;
                m_PlayerRadialProgressbar.progress = m_CircleTimerValue;
            }
        }

        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            m_PlayerRadialProgressbar = m_Root.Q<RadialProgress>(className: PLAY_SCROLL_CLASS);
            m_MouseScrollIcon = m_Root.Q<VisualElement>(classes: MOUSE_ICON);

            LoadModules();
            Invoke(nameof(AnimateMouseScroll), 1);
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            InitializeNews(m_Root);
            DoSlideNews();
        }


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
            print("[HomePanelInitializer] DoSlideNews: Started");
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
            print("[HomePanelInitializer] DoSlideNews: Finished");
        }

        private void AnimateMouseScroll()
        {
            m_MouseScrollIcon.RegisterCallback<TransitionEndEvent>(evnt =>
                m_MouseScrollIcon.ToggleInClassList("MouseScroll--Up"));
            
            m_MouseScrollIcon.ToggleInClassList("MouseScroll--Up");

        }
    }
}