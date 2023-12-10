using System;
using System.Collections;
using System.Collections.Generic;
using _Project.UI.HomePanel.Modules;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.HomePanel
{
    public class HomePanelInitializer : BaseUISection
    {
        [SerializeField] private string m_NewsScrollviewKey = "NewsContainer";
        //[SerializeField] private string m_NewsContainerKey = "NewsContainer";
        [SerializeField] private SingleNewsModule.SingleNewsData[] m_NewsArray;

        private int m_CurrentLoadedNewsIndex = 0;

        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            //InitializeNews(root);
            //DoSlideNews();

        }

        private void InitializeNews(VisualElement root)
        {
            var newsContainer = root.Q<ScrollView>(m_NewsScrollviewKey);

            foreach (SingleNewsModule.SingleNewsData singleNews in m_NewsArray)
            {
                var singleNewsModule = gameObject.AddComponent<SingleNewsModule>();
                singleNewsModule.SetData(singleNews);
                singleNewsModule.GetContainer().AddToClassList("SingleNews--Hide");
                singleNewsModule.Append(newsContainer);
                
            }
        }

        [ContextMenu("Next Slide")]
        private void DoSlideNews()
        {
            var modules = m_Root.Query(className: "SingleNews").ToList();

            if (m_CurrentLoadedNewsIndex != -1)
                modules[m_CurrentLoadedNewsIndex].AddToClassList("SingleNews--Hide");
            
            if (m_CurrentLoadedNewsIndex >= modules.Count-1)
            {
                m_CurrentLoadedNewsIndex = 0;
            }
            else
            {
                m_CurrentLoadedNewsIndex++;
            }

            modules[m_CurrentLoadedNewsIndex].RemoveFromClassList("SingleNews--Hide");
            //Invoke(nameof(DoSlideNews),5);
        }
    }
}