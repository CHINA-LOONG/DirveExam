using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIExamWindowBase : UIWindow
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
    public ButtonState btsNext;     //下一题/下一套
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
                btsNext.button.gameObject.SetActive(isLightExam);
                (btsNext.button.targetGraphic as Image).sprite = btsNext.sprSelect;
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
                btsNext.button.gameObject.SetActive(isLightExam);
                (btsNext.button.targetGraphic as Image).sprite = btsNext.sprNormal;
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

    public List<int> questions = new List<int>(5);
    public override void OnCreate()
    {
        base.OnCreate();
        btsAnswer.button.onClick.AddListener(OnClickAnswer);
        btsRandom.button.onClick.AddListener(OnClickRandom);
        btsVideo.button.onClick.AddListener(OnClickVideo);
        btsLightExam.button.onClick.AddListener(OnClickExam);
        btsRules.button.onClick.AddListener(OnClickRules);

        btsNext.button.onClick.AddListener(OnClickNext);
    }

    #region BaseFunction
    /// <summary>
    /// 点击视频教学
    /// </summary>
    void OnClickVideo()
    {

    }
    /// <summary>
    /// 点击查看新规
    /// </summary>
    void OnClickRules()
    {
        UIManager.Instance.OpenUI<UIRulesDialog>();
    }

    /// <summary>
    /// 点击显示答案
    /// </summary>
    void OnClickAnswer()
    {
        IsShowAnswer = !IsShowAnswer;
    }
    /// <summary>
    /// 点击随机题目
    /// </summary>
    void OnClickRandom()
    {
        IsLightExam = false;
        IsRandom = !IsRandom;
    }
    /// <summary>
    /// 点击灯光考试
    /// </summary>
    void OnClickExam()
    {
        IsRandom = false;
        IsLightExam = !IsLightExam;
        if (IsLightExam)
        {

        }
    }
    /// <summary>
    /// 点击下一套/下一题
    /// </summary>
    void OnClickNext()
    {
        if (IsLightExam)//灯光考试
        {

        }
        else if (IsRandom)//随机练习
        {

        }
    }
    #endregion

    #region SwitchModule
    /// <summary>
    /// 示廓灯开关
    /// </summary>
    private bool clearanceSwitch;
    public virtual bool ClearanceSwitch
    {
        get { return clearanceSwitch; }
        set
        {
                clearanceSwitch = value;
                if (value)
                {
                    HeadlightSwitch = false;
                }
        }
    }
    /// <summary>
    /// 前照灯开关
    /// </summary>
    private bool headlightSwitch;
    public virtual bool HeadlightSwitch
    {
        get { return headlightSwitch; }
        set
        {
            headlightSwitch = value;
            if (value)
            {
                ClearanceSwitch = false;
            }
        }
    }
    /// <summary>
    /// 前雾灯开关
    /// </summary>
    private bool frontFogSwitch;
    public virtual bool FrontFogSwitch
    {
        get { return frontFogSwitch; }
        set
        {
                frontFogSwitch = value;
        }
    }
    /// <summary>
    /// 后雾灯开关
    /// </summary>
    private bool rearFogSwitch;
    public virtual bool RearFogSwitch
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
    public virtual bool LeftIndicatorSwitch
    {
        get { return leftIndicatorSwitch; }
        set
        {
            if (value != leftIndicatorSwitch)
            {
                leftIndicatorSwitch = value;
                if (value)
                {
                    RightIndicatorSwitch = false;
                }
            }
        }
    }
    /// <summary>
    /// 右指示器开关
    /// </summary>
    private bool rightIndicatorSwitch;
    public virtual bool RightIndicatorSwitch
    {
        get { return rightIndicatorSwitch; }
        set
        {
            if (value != rightIndicatorSwitch)
            {
                rightIndicatorSwitch = value;
                if (value)
                {
                    LeftIndicatorSwitch = false;
                }
            }
        }
    }
    /// <summary>
    /// 双闪灯开关
    /// </summary>
    private bool doubleJumpSwitch;
    public virtual bool DoubleJumpSwitch
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
    public virtual bool FarHeadlightSwitch
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
    /// 远近切换开关
    /// </summary>
    private bool toggleHeadlightSwitch;
    public virtual bool ToggleHeadlightSwitch
    {
        get { return toggleHeadlightSwitch; }
        set
        {
            toggleHeadlightSwitch = value;
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
        get { return DoubleJumpSwitch; }
    }
    public bool ClearanceLamp
    {
        get { return ClearanceSwitch || HeadlightSwitch; }
    }
    public bool LowBeamLight
    {
        get { return HeadlightSwitch && !FarHeadlightSwitch; }
    }
    public bool HigBeamLight
    {
        get { return HeadlightSwitch && FarHeadlightSwitch; }
    }
    public bool FrontFogLamp
    {
        get { return FrontFogSwitch && (ClearanceSwitch || HeadlightSwitch); }
    }
    public bool RearFogLamp
    {
        get { return RearFogSwitch && (ClearanceSwitch || HeadlightSwitch); }
    }
    public bool LeftIndicator
    {
        get { return LeftIndicatorSwitch; }
    }
    public bool RightIndicator
    {
        get { return RightIndicatorSwitch; }
    }
    public bool LowToHigLight
    {
        get { return lowToHigCount == 2; }
    }

    #endregion

    /// <summary>
    /// 关闭所有灯光
    /// </summary>
    protected void CloseAllLight()
    {
        ClearanceSwitch = false;
        HeadlightSwitch = false;
        FrontFogSwitch = false;
        RearFogSwitch = false;
        LeftIndicatorSwitch = false;
        RightIndicatorSwitch = false;
        DoubleJumpSwitch = false;
        FarHeadlightSwitch = false;
        ToggleHeadlightSwitch = false;
    }

    void BeginLightExam()
    {
        CloseAllLight();
        //生成试题列表

    }
    void BeginExercise()
    {
        CloseAllLight();
    }

    IEnumerator _BeginQuestion(int id)
    {
        LowToHigCount = 0;//清空远近切换
        //ConfigDataMgr.in
        //AudioSystemMgr.Instance.PlaySoundByClip(ResourcesMgr.Instance.GetAudioWithStr())
        yield return null;
    }
}
