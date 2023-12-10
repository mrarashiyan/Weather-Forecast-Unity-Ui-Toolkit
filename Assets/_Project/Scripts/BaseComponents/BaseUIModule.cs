using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseUIModule : MonoBehaviour
{
    private string m_ModulePath = "<FileName>";
    
    protected VisualElement m_Root;
    protected VisualTreeAsset m_VisualAsset;
    
    protected VisualElement m_PreInsertContainer;
    protected string m_ModuleId;

    public void Awake()
    {
        GenerateId();
    }

    public void SetRoot(VisualElement root)
    {
        m_Root = root;
    }

    protected void SetModulePath(string path)
    {
        m_ModulePath = path;
    }

    public virtual void LoadModule()
    {
        if(m_VisualAsset==null)
            LoadVisualAsset();
    }

    public virtual void UnloadModule() { }
    
    public VisualElement InsertVisualElement()
    {
        m_PreInsertContainer.AddToClassList(m_ModuleId);
        m_Root.Add(m_PreInsertContainer);
        return m_PreInsertContainer;
    }

    public void DeleteVisualElement()
    {
        var selected = GetInsertedContainer();
        selected.RemoveFromHierarchy();
    }

    private void GenerateId()
    {
        m_ModuleId = Guid.NewGuid().ToString();
    }

    private void LoadVisualAsset()
    {
        m_VisualAsset = Resources.Load<VisualTreeAsset>(m_ModulePath);
    }
    
    protected T GetElement<T>(string Path) where T : VisualElement
    {
        return m_Root.Q<T>(Path);
    }
    
    public VisualElement GetPreInsertContainer()
    {
        return m_PreInsertContainer;
    }

    public VisualElement GetInsertedContainer()
    {
        var container = m_Root.Q<VisualElement>(className: m_ModuleId);
        return container;
    }

    public string GetModuleId()
    {
        return m_ModuleId;
    }
}
