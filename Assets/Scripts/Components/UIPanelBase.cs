using UnityEngine;
using System.Collections;

public class UIPanelBase : MonoBehaviour
{

    private void Awake()
    {
        BindListener();
    }
    private void OnDestroy()
    {
        UnBindListener();
    }

    protected virtual void BindListener() { }
    protected virtual void UnBindListener() { }


    public virtual void OnCreate() { }
    public virtual void OnShow() { }
    public virtual void OnHide() { }
    public virtual void OnDispose() { }
}
