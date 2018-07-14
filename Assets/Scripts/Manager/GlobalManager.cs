using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : XMonoSingleton<GlobalManager>
{
    public bool isLogin = false;

    private void Awake()
    {
        GlobalManager.Instance.isLogin = false;
        UIManager.Instance.CloseAllWindow();
        
        SceneManager.LoadScene("MainScene");
    }
}
