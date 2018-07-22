using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingWindow : UIDialog
{
    public ProgressBar progressBar;

    private AsyncOperation async;
    public void InitWith(AsyncOperation async)
    {
        this.async = async;
    }

    private void Update()
    {
        Debug.Log("进度" + async.progress);
        progressBar.Value = async.progress;
        if (async.progress>=1f)
        {
            //大众
            if (GameDataMgr.Instance.carType == CarType.DAZHONG)
            {
                UIManager.Instance.OpenUI<UIExamWindowDaZhong>();
            }
            //爱丽舍
            else
            {
                UIManager.Instance.OpenUI<UIExamWindowAiLiShe>();
            }
            UIManager.Instance.CloseUI(this);
        }
    }
}
