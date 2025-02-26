using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    private UIManager UIManager => UIManager.Instance;

    public event Action OnOpen; 
    public event Action OnClose; 
    
    public void CloseWindow()
    {
        OnClose?.Invoke();
        gameObject.SetActive(false);
    }
    
    public void OpenWindow()
    {
        OnOpen?.Invoke();
        gameObject.SetActive(true);
    }

    protected void OpenWindow(string windowName)
    {
        UIManager.OpenWindow(windowName);
    }
}
