using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainWindow : UIWindow
{
    public Button btnJieda;
    public Button btnAilishe;


    public override void OnCreate()
    {
        base.OnCreate();
        btnJieda.onClick.AddListener(OnClickJieda);
        btnAilishe.onClick.AddListener(OnClickAilishe);
    }

    /// <summary>
    /// Ons the click jieda.
    /// </summary>
    void OnClickJieda()
    {
        UIDetailWindow uiDetailWindow = UIManager.Instance.OpenWindow<UIDetailWindow>();
    }
    /// <summary>
    /// Ons the click ailishe.
    /// </summary>
    void OnClickAilishe()
    {
        UIDetailWindow uiDetailWindow = UIManager.Instance.OpenWindow<UIDetailWindow>();
    }
}
