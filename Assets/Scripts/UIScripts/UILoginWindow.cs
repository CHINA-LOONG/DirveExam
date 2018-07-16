using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginWindow : UIWindow
{
    public Text textState;

    [System.Serializable]
    public class LoginGroup
    {
        public GameObject root;
        public InputField iptAccount;
        public InputField iptPassword;
        public Button btnLogin;
        public Button btnRegister;
        public Button btnWechat;
    }
    public LoginGroup loginGroup;

    [System.Serializable]
    public class ProgressGroup
    {
        public GameObject root;
        public Text textProg;
        public ProgressBar progressBar;
    }
    public ProgressGroup progressGroup;


    public enum State
    {
        None,
        Login,
        Update
    }
    private State uiState = State.None;

    public State UiState
    {
        get { return uiState; }
        set
        {
            if (uiState != value)
            {
                uiState = value;
                loginGroup.root.SetActive(uiState == State.Login);
                progressGroup.root.SetActive(uiState == State.Update);
            }
        }
    }

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
        loginGroup.btnLogin.onClick.AddListener(OnClickLogin);
        loginGroup.btnWechat.onClick.AddListener(OnClickWechat);
        loginGroup.btnRegister.onClick.AddListener(OnClickRegister);
    }


    public void SetLoginList()
    {
        UiState = State.Login;
        SetState("");
    }
    public void SetProgress(float prog)
    {
        UiState = State.Update;
        progressGroup.textProg.text = prog.ToString("P");
        progressGroup.progressBar.Value = prog;
    }
    public void SetState(string strState)
    {
        textState.text = strState;
    }


    void OnClickLogin()
    {
        UIManager.Instance.OpenWindow<UIMainWindow>();
    }
    void OnClickWechat()
    {

    }
    void OnClickRegister()
    {

    }
}
