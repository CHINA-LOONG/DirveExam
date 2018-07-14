using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginWindow : UIWindow
{
    public enum State{
        None,
        Normal,
        Login,
        Update
    }

    public Button button;
    public AudioSource audioSource;

    protected override void BindListener()
    {
        base.BindListener();
    }
    protected override void UnBindListener()
    {
        base.UnBindListener();
    }

    public override void OnCreate()
    {
        base.OnCreate();
        button.onClick.AddListener(OnClickButton);
    }

    void OnClickButton()
    {
        audioSource.clip = ResourcesMgr.Instance.GetAudioWithStr(ConfigDataMgr.ExamStart);
        audioSource.Play();
    }
}
