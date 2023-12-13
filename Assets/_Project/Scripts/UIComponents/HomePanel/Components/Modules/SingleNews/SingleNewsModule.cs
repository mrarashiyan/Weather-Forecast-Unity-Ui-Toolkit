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
        // path from the Resource folder
        private const string MODULE_PATH = "VisualTrees/SingleNews/NewsModule";
        
        // Selectors cons for finding Elements in UXML
        private const string THUMBNAIL_IMAGE = "Thumbnail";
        private const string TITLE_LABEL = "Title";
        private const string DESC_LABEL = "Desc";
        private const string TAGS_LABEL = "Tags";
        private const string LINK_LABEL = "Link";

        // Data Model that we should read data from it
        private SingleNews m_CurrentSingleNews;
        
        public void SetData(SingleNews singleNewsData)
        {
            // Set Data Model Object
            m_CurrentSingleNews = singleNewsData;
        }

        public override void LoadModule()
        {
            // Set Module Path and Load the VisualTreeAsset
            SetModulePath(MODULE_PATH);
            
            base.LoadModule();

            // prepare the Module and load data from Data Model to the correct Element
            GetPreInsertContainer().Q<VisualElement>(THUMBNAIL_IMAGE).style.backgroundImage = new StyleBackground(m_CurrentSingleNews.thumbnail);
            GetPreInsertContainer().Q<Label>(TITLE_LABEL).text = m_CurrentSingleNews.title;
            GetPreInsertContainer().Q<Label>(DESC_LABEL).text = m_CurrentSingleNews.desc;
            GetPreInsertContainer().Q<Label>(TAGS_LABEL).text = m_CurrentSingleNews.tags;
            GetPreInsertContainer().Q<Label>(LINK_LABEL).text = m_CurrentSingleNews.link;
        }

    }
}