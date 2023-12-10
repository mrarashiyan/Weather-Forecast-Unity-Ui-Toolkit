using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.HomePanel.Modules
{
    public class SingleNewsModule : MonoBehaviour
    {
        [Serializable]
        public class SingleNewsData
        {
            public Texture2D thumbnail;
            public string title;
            public string desc;
            public string tags;
            public string link;
        }
        
        [SerializeField] private VisualTreeAsset m_VisualAsset;

        private const string THUMBNAIL_IMAGE = "Thumbnail";
        private const string TITLE_LABEL = "Title";
        private const string DESC_LABEL = "Desc";
        private const string TAGS_LABEL = "Tags";
        private const string LINK_LABEL = "Link";

        private VisualElement m_templateContainer;

        public void SetData(SingleNewsData singleNewsData)
        {
            m_templateContainer = m_VisualAsset.Instantiate().Q<VisualElement>("SingleNews");
            m_templateContainer.Q<VisualElement>(THUMBNAIL_IMAGE).style.backgroundImage = new StyleBackground(singleNewsData.thumbnail);
            m_templateContainer.Q<Label>(TITLE_LABEL).text = singleNewsData.title;
            m_templateContainer.Q<Label>(DESC_LABEL).text = singleNewsData.desc;
            m_templateContainer.Q<Label>(TAGS_LABEL).text = singleNewsData.tags;
            m_templateContainer.Q<Label>(LINK_LABEL).text = singleNewsData.link;
        }

        public VisualElement Append(VisualElement root)
        {
            root.Add(m_templateContainer);
            return m_templateContainer;
        }

        public VisualElement GetContainer()
        {
            return m_templateContainer;
        }

    }
}