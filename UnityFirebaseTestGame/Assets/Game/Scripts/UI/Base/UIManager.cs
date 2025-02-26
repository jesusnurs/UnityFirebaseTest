using System.Collections.Generic;
using Game.Scripts.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private List<UIWindow> _windows = new List<UIWindow>();
    [SerializeField] private List<string> _openOnStartWindows;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        foreach (var windowName in _openOnStartWindows)
        {
            OpenWindow(windowName);
        }
    }

    public void CloseWindow(string windowName)
    {
        foreach (var window in _windows)
        {
            if (window.WindowName == windowName)
            {
                window.UIElement.CloseWindow();
                break;
            }
        }
    }

    public void OpenWindow(string windowName)
    {
        foreach (var window in _windows)
        {
            if (window.WindowName == windowName)
            {
                window.UIElement.OpenWindow();
                break;
            }
        }
    }
}