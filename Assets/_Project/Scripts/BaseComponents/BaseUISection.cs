using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// To Use this class, always follow these steps:
/// 1- Override Initialize method (Make sure you are running Base.Initialize() in overrided method)
/// 2- Override LoadModules method
/// 3- Use AddModule inside your LoadModule function and call in inside your Initialize method.
/// 4- After panel got accessible, Call EnterSection (You can override it)
/// 5- When panel is left, call ExitSection (You can override it)
/// </summary>
public class BaseUISection : MonoBehaviour
{
    [Header("Path of Current Section in Root UXML (Leave a '.' character for Root node)")]
    [Tooltip("It can be one simple ID or a path to a certain element")]
    [SerializeField]
    protected string m_SectionUxmlPath;

    /// <summary>
    /// Root Visual Element of UXML file
    /// </summary>
    protected VisualElement m_Root;

    /// <summary>
    /// Container which this section is starting from it in UXML file
    /// </summary>
    protected VisualElement m_SectionContainer;

    private bool m_IsActive = false;

    #region Initializers

    /// <summary>
    /// First function which is run from this Section.
    /// You have to do your first Initializations or Query Selectors here.
    /// It's like Start method in MonoBehavior
    /// </summary>
    /// <param name="root"></param>
    public virtual void Initialize(VisualElement root)
    {
        SetRoot(root);
        FindSectionPath();
    }

    /// <summary>
    ///This function is called automatically in OnDestroy method.
    /// if you have to unload your modules or save any data, override this method
    /// </summary>
    public virtual void Deinitialize()
    {
    }

    /// <summary>
    /// if you want to change the Root manually, use this function
    /// </summary>
    /// <param name="root"></param>
    public void SetRoot(VisualElement root)
    {
        if (root == null)
        {
            Debug.LogError("[BaseUISection] SetRoot: Root is null!", this);
            return;
        }

        m_Root = root;
    }

    private void FindSectionPath()
    {
        if(m_SectionUxmlPath == String.Empty)
            Debug.LogError(string.Format("[BaseUiSection:{0}] FindSectionPath: SectionUxmlPath Is Empty",gameObject.name));

        if (m_SectionUxmlPath == ".")
            m_SectionContainer = m_Root;
        else
        {
            var searchContainer = m_Root;
            foreach (var keyword in m_SectionUxmlPath.Split(" "))
            {
                if (keyword.StartsWith("."))
                    searchContainer = searchContainer.Q(className: keyword);
                else
                    searchContainer = searchContainer.Q(keyword);

                if (searchContainer == null)
                {
                    Debug.LogError(string.Format("[BaseUiSection:{0}] FindSectionPath: Keyword={1} Not Found in Root",
                        gameObject.name, keyword));
                }
            }

            m_SectionContainer = searchContainer;
        }
    }

    #endregion

    #region Modules Controller

    /// <summary>
    /// Load any module of your Section. You have to run it manually
    /// Override this function if you have any Module in your Ui Section
    /// </summary>
    protected virtual void LoadModules()
    {
    }

    protected virtual void UnloadModules()
    {
    }

    #endregion

    #region Modules Utility

    /// <summary>
    /// Use this function to Load any BaseUiModule in this Section.
    /// After you use this function you have to Initialize Modules then.
    /// </summary>
    /// <typeparam name="T">BaseUiModule</typeparam>
    /// <returns></returns>
    protected T AddModule<T>() where T : BaseUIModule
    {
        var component = gameObject.AddComponent<T>();
        return component;
    }

    protected T[] GetModules<T>() where T : BaseUIModule
    {
        var components = gameObject.GetComponents<T>();
        return components;
    }

    #endregion

    #region Active/Deactivate a Section

    public virtual void EnterSection()
    {
        m_IsActive = true;
    }

    public virtual void ExitSection(bool autoUnloadModules = false)
    {
        m_IsActive = false;
        if (autoUnloadModules)
            UnloadModules();
    }

    public bool GetSectionActivationStatus()
    {
        return m_IsActive;
    }

    #endregion

    /// <summary>
    /// Get VisualElement of the current Section from UXML file.
    /// </summary>
    /// <returns></returns>
    public VisualElement GetSectionContainer()
    {
        return m_SectionContainer;
    }
}