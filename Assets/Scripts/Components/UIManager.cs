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

    public T OpenWindow<T>() where T : UIPanelBase
    {
        System.Type type = typeof(T);

        GameObject prefab = ResourcesMgr.Instance.LoadUIPrefab(type.ToString());
        if (type.IsSubclassOf(typeof(UIWindow)))
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, wndParent);
            UIWindow wnd = go.GetComponent<UIWindow>();
            wnd.OnCreate();
            //创建新的主界面时，删除所有界面
            if (wnd.isMain)
            {
                while (windowsStack.Count > 0)
                {
                    UIWindow temp = windowsStack.Pop();
                    temp.OnDispose();
                    Destroy(temp.gameObject);
                }
            }
            wnd.OnShow();
            windowsStack.Push(wnd);
            return wnd as T;
        }
        else if (type.IsSubclassOf(typeof(UIDialog)))
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, dlgParent);
            UIDialog dlg = go.GetComponent<UIDialog>();
            dlg.OnCreate();
            dlg.OnShow();
            dialogsStack.Push(dlg);
            return dlg as T;
        }
        else
        {
            Debug.LogError("打開的UI類型有問題，請檢測類型");
            return null;
        }
    }

    public void CloseAllWindow(){
        
    }

}
