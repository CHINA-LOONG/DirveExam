using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : XMonoSingleton<UIManager>
{
    public Transform wndParent;
    public Transform dlgParent;


    public Stack<UIWindow> windowsStack = new Stack<UIWindow>();
    public Stack<UIDialog> dialogsStack = new Stack<UIDialog>();

    public override void OnInit()
    {
        base.OnInit();
    }

    public void OpenWindow<T>() where T : UIPanelBase
    {
        System.Type type = typeof(T);

        GameObject prefab = ResourcesMgr.Instance.LoadUIPrefab(type.ToString());
        if (type.IsSubclassOf(typeof(UIWindow)))
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, wndParent);
            T wnd = go.GetComponent<T>();

            windowsStack.Push(wnd as UIWindow);
            //Debug.Log("打開界面："+type.ToString());
        }
        else if (type.IsSubclassOf(typeof(UIDialog)))
        {
            //Debug.Log("打開彈框："+type.ToString());
        }
        else
        {
            Debug.LogError("打開的UI類型有問題，請檢測類型");
        }
    }

    public void CloseAllWindow(){
        
    }

}
