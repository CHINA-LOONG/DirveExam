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
    /// 检测是是否进入视频界面
    /// </summary>
    void ShowDetailWindow()
    {
        List<VideoData> videoDatas;
        if (ConfigDataMgr.Instance.gameConfig.video.ContainsKey(GameDataMgr.Instance.carType.ToString()))
        {
            videoDatas = ConfigDataMgr.Instance.gameConfig.video[GameDataMgr.Instance.carType.ToString()];
        }
        else
        {
            videoDatas = new List<VideoData>();
        }

        if (videoDatas.Count > 0)
        {
            UIDetailWindow uiDetailWindow = UIManager.Instance.OpenUI<UIDetailWindow>();
        }
        else
        {
            SwitchSceneMgr.Instance.SwitchToExam();
        }
    }

    /// <summary>
    /// Ons the click jieda.
    /// </summary>
    void OnClickJieda()
    {
        GameDataMgr.Instance.carType = CarType.DAZHONG;
        ShowDetailWindow();
    }
    /// <summary>
    /// Ons the click ailishe.
    /// </summary>
    void OnClickAilishe()
    {
        GameDataMgr.Instance.carType = CarType.AILISHE;
        ShowDetailWindow();
    }
}
