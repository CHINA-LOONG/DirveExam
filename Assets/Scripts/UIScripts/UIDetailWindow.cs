using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDetailWindow : UIWindow
{
    public Button btnReturn;
    public Button btnStart;

    public override void OnCreate()
    {
        base.OnCreate();
        btnReturn.onClick.AddListener(OnClickReturn);
        btnStart.onClick.AddListener(OnClickStart);
    }

    /// <summary>
    /// Ons the click return.
    /// </summary>
    void OnClickReturn()
    {
        UIManager.Instance.CloseWindow(this);
    }
    /// <summary>
    /// Ons the click start.
    /// </summary>
    void OnClickStart()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("ExamScene");
    }
}
