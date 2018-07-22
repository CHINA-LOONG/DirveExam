using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRulesDialog : UIDialog
{
    public Button btnClose;
    // Use this for initialization
    void Start()
    {
        btnClose.onClick.AddListener(OnClickClose);
    }

    void OnClickClose(){
        UIManager.Instance.CloseUI(this);
    }
}
