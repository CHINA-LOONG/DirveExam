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

    public T OpenUI<T>() where T : UIPanelBase
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
            else if (windowsStack.Count > 0)
            {
                UIWindow temp = windowsStack.Peek();
                temp.OnHide();
                temp.gameObject.SetActive(false);
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

    public void CloseUI(UIPanelBase uiPanel)
    {
        if (uiPanel.GetType().IsSubclassOf(typeof(UIWindow)))
        {
            if (!(uiPanel as UIWindow).isMain && windowsStack.Peek() == uiPanel)
            {
                windowsStack.Pop();
                Destroy(uiPanel.gameObject);
                UIWindow temp = windowsStack.Peek();
                temp.gameObject.SetActive(true);
                temp.OnShow();
            }
            else
            {
                Debug.LogError("要关闭的界面不是栈顶元素或是栈底元素");
            }
        }
        else if (uiPanel.GetType().IsSubclassOf(typeof(UIDialog)))
        {
            if (dialogsStack.Peek() == uiPanel)
            {
                dialogsStack.Pop();
                Destroy(uiPanel.gameObject);
                if (dialogsStack.Count > 0)
                {
                    dialogsStack.Peek().OnShow();
                }
            }
            else
            {
                Debug.LogError("要关闭的界面不是栈顶元素");
            }
        }
        else
        {
            Debug.LogError("关闭的UI類型有問題，請檢測類型");
        }
    }

    public void CloseAllWindow()
    {

    }

}
