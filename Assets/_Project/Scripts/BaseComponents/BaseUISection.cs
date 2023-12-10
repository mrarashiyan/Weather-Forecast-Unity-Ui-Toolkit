using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BaseUISection : MonoBehaviour
{
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

    protected virtual void LoadModules()
    {
    }

    protected T AddModule<T>() where T : Component
    {
        var component = gameObject.AddComponent<T>();
        return component;
    }

    protected T[] GetModules<T>() where T : Component
    {
        var components = gameObject.GetComponents<T>();
        return components;
    }
}