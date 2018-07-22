using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneMgr : XSingleton<SwitchSceneMgr>
{
    public void SwitchToExam(){
        AsyncOperation async = SceneManager.LoadSceneAsync("ExamScene");
        UILoadingWindow uiLoadingWindow = UIManager.Instance.OpenUI<UILoadingWindow>();
        uiLoadingWindow.InitWith(async);
    }
}
