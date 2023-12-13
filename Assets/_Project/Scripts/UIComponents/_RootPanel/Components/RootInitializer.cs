using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.UI.HomePanel
{
    public class RootInitializer : BaseUISection
    {
        private const string BODY_SCROLLBAR = "BodyScroll";
        private const string BURGER_BUTTON = "BurgerBtn";
        private const string LEFT_MENU = "LeftMenu";

        private Button m_BurgerBtn;
        private VisualElement m_LeftMenu;
        private ScrollView m_ScrollView;
        private bool m_ToggleLeftMenu = false;
        
        
        public override void Initialize(VisualElement root)
        {
            base.Initialize(root);

            m_BurgerBtn = root.Q<Button>(className: BURGER_BUTTON);
            m_LeftMenu = root.Q<VisualElement>(className:LEFT_MENU);
            m_ScrollView = m_Root.Q<ScrollView>(BODY_SCROLLBAR);
            
            m_BurgerBtn.clicked += ToggleLeftMenu;
        }

        public override void Deinitialize()
        {
            base.Deinitialize();
            
            m_BurgerBtn.clicked -= ToggleLeftMenu;
        }

        private void ToggleLeftMenu()
        {
            m_ToggleLeftMenu = !m_ToggleLeftMenu;
            if(m_ToggleLeftMenu)
                m_LeftMenu.RemoveFromClassList("LeftMenu--Closed");
            else
                m_LeftMenu.AddToClassList("LeftMenu--Closed");
        }

        public void ScrollTo(VisualElement target)
        {
            m_ScrollView.ScrollTo(target);
        }
    }
}