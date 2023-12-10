using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.BaseComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements;

[RequireComponent(typeof(InjectorEditorUpdater))]
public class BaseUIInjector : MonoBehaviour
{
    [SerializeField] protected bool m_LoadOnEnable;
    [SerializeField] protected bool m_UnloadOnDisable;

    [SerializeField] protected UIDocument m_ParentUiDocument;
    [SerializeField] protected string m_PlaceholderId;
    [SerializeField] protected BaseUISection m_PrefabToInject;

    protected VisualElement m_Root;
    protected BaseUISection m_InstantiatedSection;
    protected TemplateContainer m_SectionContainer;

    #region Engine Functions
    private void OnEnable()
    {
        if (m_LoadOnEnable && Application.isPlaying == true)
            Initialize();
    }

    
    private void OnDisable()
    {
        if (m_UnloadOnDisable && Application.isPlaying == true)
            Deinitialize();
    }

    private void OnDestroy()
    {
        Deinitialize();
    }

    #endregion

    #region Public Functions

    public virtual void Initialize()
    {
        print(string.Format("[{0}] Initialize : Start", gameObject.name));
        SetRoot(m_ParentUiDocument.rootVisualElement);

        InstantiatePrefab();
        LoadSection();

        m_InstantiatedSection?.Initialize(m_Root);
    }

    public virtual void Deinitialize()
    {
        DestroyPrefab();
        UnloadSection();

        m_InstantiatedSection?.Deinitialize();
    }

    #endregion

    #region Injection

    private void InstantiatePrefab()
    {
        if (m_InstantiatedSection != null)
            return;

        m_InstantiatedSection = Instantiate(m_PrefabToInject.gameObject, transform).GetComponent<BaseUISection>();
    }

    private void LoadSection()
    {
        var placeHolder = GetElement<VisualElement>(m_PlaceholderId);
        m_SectionContainer = m_InstantiatedSection.GetPrefabUiDocument().Instantiate();
        placeHolder.Add(m_SectionContainer);
    }

    protected void UnloadSection()
    {
        if (m_SectionContainer == null)
            return;

        var placeHolder = GetElement<VisualElement>(m_PlaceholderId);
        placeHolder.Remove(m_SectionContainer);
    }


    private void DestroyPrefab()
    {
        if (m_InstantiatedSection == null)
            return;

        if (Application.isPlaying)
            Destroy(m_InstantiatedSection.gameObject);
        else
            DestroyImmediate(m_InstantiatedSection.gameObject);
    }

    #endregion

    #region Utility

    private void SetRoot(VisualElement root)
    {
        m_Root = root;
    }


    protected T GetElement<T>(string Path) where T : VisualElement
    {
        return m_Root.Q<T>(Path);
    }

    public void UpdateEditor(string fileName)
    {
        Deinitialize();
        Initialize();
        m_InstantiatedSection.MarkAsEditorObject(gameObject);
    }

    #endregion
}