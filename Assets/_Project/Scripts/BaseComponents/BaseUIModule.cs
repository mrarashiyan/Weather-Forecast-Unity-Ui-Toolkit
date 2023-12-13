using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// How To Use this Class:
/// ==> When You are Overriding:
/// 1-Override LoadModule and set all of the data from your DataModel here (Don't forget to keep Base.LoadModule())
/// 2- Call SetModulePath inside LoadModule (before Base.LoadModule()) to set path of VisualTree Asset inside Resource folder
/// 3- Override UnloadModule if you want to do any specific function when your module is removing or deleteing
/// 
/// ==> When You are using:
/// 1- call SetRoot function inside LoadModule to set the root
/// 2- Call LoadModule to load the Module dynamically from ModulePath
/// 3- call InsertVisualElement to add the element to the Root
/// </summary>
public class BaseUIModule : MonoBehaviour
{
    private string m_ModulePath = "<FileName>";
    
    protected VisualElement m_Root;
    private VisualTreeAsset m_VisualAsset;
    
    private VisualElement m_PreInsertContainer;
    private VisualElement m_InsertedContainer;
    
    /// <summary>
    /// Every Module has an Specific ModuleId which is unique and can be retrived in future
    /// </summary>
    protected string m_ModuleId;

    public void Awake()
    {
        GenerateId();
    }

    /// <summary>
    /// Set Visual Element which the Module will instantiate as child of it
    /// </summary>
    /// <param name="root"></param>
    public void SetRoot(VisualElement root)
    {
        m_Root = root;
    }

    /// <summary>
    /// Set path of the VisualTree asset in Resource folder
    /// </summary>
    /// <param name="path"></param>
    protected void SetModulePath(string path)
    {
        m_ModulePath = path;
    }

    /// <summary>
    /// Load the module and set all fields inside DataModel
    /// </summary>
    public virtual void LoadModule()
    {
        if(m_VisualAsset==null)
            LoadVisualAsset();
    }

    public virtual void UnloadModule()
    {
        DeleteVisualElement();
        Destroy(this);
    }
    
    /// <summary>
    /// Insert module form PreInsertContainer to Root variable and assign a Unique id to it
    /// </summary>
    /// <returns></returns>
    public VisualElement InsertVisualElement()
    {
        // Copy all stylesheets of visual asset to the module
        foreach (var styleSheet in m_VisualAsset.stylesheets)
        {
            m_PreInsertContainer.styleSheets.Add(styleSheet);
        }
        
        m_PreInsertContainer.AddToClassList(m_ModuleId);
        m_Root.Add(m_PreInsertContainer);
        m_InsertedContainer = m_Root.Q<VisualElement>(className: m_ModuleId);
        return m_PreInsertContainer;
    }

    /// <summary>
    /// Delete this module and remove from Root
    /// </summary>
    public void DeleteVisualElement()
    {
        var selected = GetInsertedContainer();
        selected.RemoveFromHierarchy();
    }

    /// <summary>
    /// Generate a unique id for the module
    /// </summary>
    private void GenerateId()
    {
        m_ModuleId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Load tje VisualTree Asset from Resource folder
    /// </summary>
    private void LoadVisualAsset()
    {
        m_VisualAsset = Resources.Load<VisualTreeAsset>(m_ModulePath);
        if (m_VisualAsset == null)
        {
            print(string.Format("[{0}] LoadVisualAsset: Error, Visual Assets Not Found", gameObject.name));
            return;
        }

        // Load m_VisualAsset into PreInsertContainer and ignore the TempContainer module
        m_PreInsertContainer = m_VisualAsset.Instantiate().ElementAt(0);
    }
    
    protected T GetElement<T>(string Path) where T : VisualElement
    {
        return m_Root.Q<T>(Path);
    }
    
    /// <summary>
    /// Get the module UXML before adding to the Root
    /// </summary>
    /// <returns></returns>
    public VisualElement GetPreInsertContainer()
    {
        return m_PreInsertContainer;
    }

    /// <summary>
    /// Get the module UXML which is exist in the Root
    /// </summary>
    /// <returns></returns>
    public VisualElement GetInsertedContainer()
    {
        return m_InsertedContainer;
    }

    public string GetModuleId()
    {
        return m_ModuleId;
    }
}
