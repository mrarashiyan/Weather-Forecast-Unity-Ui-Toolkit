using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using _Project.UI.HomePanel.Modules.DataModels;

namespace _Project.UI.HomePanel.Modules
{
    public class SingleNewsModule : BaseUIModule
    {
        private string MODULE_PATH = "VisualTrees/SingleNews/NewsModule";
        
        private const string THUMBNAIL_IMAGE = "Thumbnail";
        private const string TITLE_LABEL = "Title";
        private const string DESC_LABEL = "Desc";
        private const string TAGS_LABEL = "Tags";
        private const string LINK_LABEL = "Link";

        private SingleNews m_CurrentSingleNews;
        
        public void SetData(SingleNews singleNewsData)
        {
            m_CurrentSingleNews = singleNewsData;
        }

        public override void LoadModule()
        {
            SetModulePath(MODULE_PATH);
            
            base.LoadModule();

            m_PreInsertContainer = m_VisualAsset.Instantiate().Q<VisualElement>("SingleNews");
            m_PreInsertContainer.Q<VisualElement>(THUMBNAIL_IMAGE).style.backgroundImage = new StyleBackground(m_CurrentSingleNews.thumbnail);
            m_PreInsertContainer.Q<Label>(TITLE_LABEL).text = m_CurrentSingleNews.title;
            m_PreInsertContainer.Q<Label>(DESC_LABEL).text = m_CurrentSingleNews.desc;
            m_PreInsertContainer.Q<Label>(TAGS_LABEL).text = m_CurrentSingleNews.tags;
            m_PreInsertContainer.Q<Label>(LINK_LABEL).text = m_CurrentSingleNews.link;
        }

    }
}