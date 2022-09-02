using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UIWindow : MonoBehaviour {

    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler OnClose;

    public virtual Type Type { get { return this.GetType(); } }

    public GameObject Root;

    public enum WindowResult
    {
        None = 0,
        Yes,
        No,
    }
	
    public void Close (WindowResult result = WindowResult.No)
    {
        UIManager.Instance.Close(this.Type);
        if (this.OnClose != null)
            this.OnClose(this, result);
        this.OnClose = null;
    }

    public virtual void OnCloseClick()
    {
        this.Close();
    }
    public virtual void OnYesClick()
    {
        this.Close(WindowResult.Yes);
    }

    public virtual void OnNoClick()
    {
        this.Close(WindowResult.No);
    }

    private void OnMouseDown()
    {
        Debug.LogFormat(this.name + " Clicked");
    }
}
