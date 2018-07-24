using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITipsDialog : UIDialog
{
    public static void ShowTips(string tips)
    {
        UITipsDialog uITipsDialog = UIManager.Instance.OpenUI<UITipsDialog>();
        uITipsDialog.InitWith(tips);
    }
    public Text textTips;

    public void InitWith(string tips)
    {
        textTips.text = tips;

        XTime.Instance.AddTimer(3f, 1, () =>
        {
            UIManager.Instance.CloseUI(this);
        });
    }
}
