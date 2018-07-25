using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExamWindowBase : UIWindow
{
    [System.Serializable]
    public class ButtonState
    {
        public Button button;
        public Image image { get { return button.targetGraphic as Image; } }
        public Sprite sprNormal;
        public Sprite sprSelect;
    }

    public ButtonState btsAnswer;   //显示答案
    public ButtonState btsRandom;   //随机题目
    public ButtonState btsVideo;    //视频讲解
    public ButtonState btsLightExam;//灯光考试
    public ButtonState btsRules;    //新规内容

    public Button btnNext;
    public Button btnHelp;          //内容提示

    public Text textQuestion;
    public Image imgResult;
    public Text textAnswer;

    private bool isShowVideo;   //显示视频
    private bool isNewRules;    //显示新规
    public bool IsShowVideo
    {
        get { return isShowVideo; }
        set
        {
            if (isShowVideo != value)
            {
                isShowVideo = value;
                btsVideo.image.sprite = value ? btsVideo.sprSelect : btsVideo.sprNormal;
            }
        }
    }
    public bool IsNewRules
    {
        get { return isNewRules; }
        set
        {
            if (isNewRules != value)
            {
                isNewRules = value;
                btsRules.image.sprite = value ? btsRules.sprSelect : btsRules.sprNormal;
            }
        }
    }

    private bool isRandom;      //随机题目
    private bool isLightExam;   //灯光考试
    private bool isShowAnswer;  //显示答案
    public bool IsRandom
    {
        get { return isRandom; }
        set
        {
            if (isRandom != value)
            {
                isRandom = value;
                btsRandom.image.sprite = value ? btsRandom.sprSelect : btsRandom.sprNormal;
            }
        }
    }
    public bool IsLightExam
    {
        get { return isLightExam; }
        set
        {
            if (isLightExam != value)
            {
                isLightExam = value;
                btsLightExam.image.sprite = value ? btsLightExam.sprSelect : btsLightExam.sprNormal;
            }
        }
    }
    public bool IsShowAnswer
    {
        get { return isShowAnswer; }
        set
        {
            if (isShowAnswer != value)
            {
                isShowAnswer = value;
                btsAnswer.image.sprite = value ? btsAnswer.sprSelect : btsAnswer.sprNormal;
            }
        }
    }

    public override void OnCreate()
    {
        base.OnCreate();
        btsAnswer.button.onClick.AddListener(OnClickAnswer);
        btsRandom.button.onClick.AddListener(OnClickRandom);
        btsVideo.button.onClick.AddListener(OnClickVideo);
        btsLightExam.button.onClick.AddListener(OnClickExam);
        btsRules.button.onClick.AddListener(OnClickRules);
    }

    /// <summary>
    /// 点击显示答案
    /// </summary>
    protected virtual void OnClickAnswer()
    {

    }
    /// <summary>
    /// 点击随机题目
    /// </summary>
    protected virtual void OnClickRandom()
    {

    }
    /// <summary>
    /// 点击视频教学
    /// </summary>
    void OnClickVideo()
    {

    }
    /// <summary>
    /// 点击灯光考试
    /// </summary>
    void OnClickExam()
    {

    }
    /// <summary>
    /// 点击查看新规
    /// </summary>
    void OnClickRules()
    {

    }


    #region SwitchModule
    /// <summary>
    /// 示廓灯开关
    /// </summary>
    private bool clearanceSwitch;
    public bool ClearanceSwitch
    {
        get { return clearanceSwitch; }
        set
        {
            if (value != clearanceSwitch)
            {
                clearanceSwitch = value;
            }
        }
    }
    /// <summary>
    /// 前照灯开关
    /// </summary>
    private bool headlightSwitch;
    public bool HeadlightSwitch
    {
        get { return headlightSwitch; }
        set
        {
            if (value != headlightSwitch)
            {
                headlightSwitch = value;

            }
        }
    }
    /// <summary>
    /// 前雾灯开关
    /// </summary>
    private bool frontFogSwitch;
    public bool FrontFogSwitch
    {
        get { return frontFogSwitch; }
        set
        {
            if (value != frontFogSwitch)
            {
                frontFogSwitch = value;
            }
        }
    }
    /// <summary>
    /// 后雾灯开关
    /// </summary>
    private bool rearFogSwitch;
    public bool RearFogSwitch
    {
        get { return rearFogSwitch; }
        set
        {
            if (value != rearFogSwitch)
            {
                rearFogSwitch = value;
            }
        }
    }
    /// <summary>
    /// 左指示器开关
    /// </summary>
    private bool leftIndicatorSwitch;
    public bool LeftIndicatorSwitch
    {
        get { return leftIndicatorSwitch; }
        set
        {
            if (value != leftIndicatorSwitch)
            {
                leftIndicatorSwitch = value;

            }
        }
    }
    /// <summary>
    /// 右指示器开关
    /// </summary>
    private bool rightIndicatorSwitch;
    public bool RightIndicatorSwitch
    {
        get { return rightIndicatorSwitch; }
        set
        {
            if (value != rightIndicatorSwitch)
            {
                rightIndicatorSwitch = value;

            }
        }
    }
    /// <summary>
    /// 双闪灯开关
    /// </summary>
    private bool doubleJumpSwitch;
    public bool DoubleJumpSwitch
    {
        get { return doubleJumpSwitch; }
        set
        {
            if (value != doubleJumpSwitch)
            {
                doubleJumpSwitch = value;
            }
        }
    }
    /// <summary>
    /// 远光大灯开关
    /// </summary>
    private bool farHeadlightSwitch;//默认是近光状态
    public bool FarHeadlightSwitch
    {
        get { return farHeadlightSwitch; }
        set
        {
            if (value != farHeadlightSwitch)
            {
                farHeadlightSwitch = value;
            }
        }
    }
    /// <summary>
    /// 远近切换计数
    /// </summary>
    private int lowToHigCount = 0;
    public int LowToHigCount
    {
        get { return lowToHigCount; }
        set
        {
            if (value == 0 || (value - 1) == lowToHigCount)
            {
                lowToHigCount = value;
            }
        }
    }
    #endregion

    #region LightStatus
    public bool DoubleJumpLamp
    {
        get
        {
            return true;
        }
    }
    public bool ClearanceLamp
    {
        get { return true; }
    }
    public bool LowBeamLight
    {
        get { return true; }
    }
    public bool HigBeamLight
    {
        get { return true; }
    }
    public bool FrontFogLamp
    {
        get { return true; }
    }
    public bool RearFogLamp
    {
        get { return true; }
    }
    public bool LeftIndicator
    {
        get { return true; }
    }
    public bool RightIndicator
    {
        get { return true; }
    }
    public bool LowToHigLight
    {
        get { return true; }
    }
    #endregion
}
