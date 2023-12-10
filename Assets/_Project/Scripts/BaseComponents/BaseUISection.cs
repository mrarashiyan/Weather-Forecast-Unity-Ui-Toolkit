using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.BaseComponents;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UIElements;


public class BaseUISection : MonoBehaviour
{
    [SerializeField] protected VisualTreeAsset m_PrefabUiDocument;
    protected VisualElement m_Root;

    public virtual void Initialize(VisualElement root)
    {
        SetRoot(root);
    }

    public virtual void Deinitialize()
    {
    }

    public void SetRoot(VisualElement root)
    {
        if (root == null)
        {
            Debug.LogError("[BaseUISection] SetRoot: Root is null!", this);
            return;
        }

        m_Root = root;
    }

    public VisualTreeAsset GetPrefabUiDocument()
    {
        return m_PrefabUiDocument;
    }

    public void MarkAsEditorObject(GameObject reference)
    {
        gameObject.name = "[EditorOnly] " + gameObject.name;
        gameObject.AddComponent<SectionEditorObject>().SetReference(reference);
    }

    protected virtual void LoadModules()
    {
    }

    protected T GetElement<T>(string Path) where T : VisualElement
    {
        return m_Root.Q<T>(Path);
    }

    protected T AddModule<T>() where T : Component
    {
        var component = gameObject.AddComponent<T>();
        return component;
    }

    protected T GetModule<T>() where T : Component
    {
        var component = gameObject.GetComponent<T>();
        return component;
    }
}