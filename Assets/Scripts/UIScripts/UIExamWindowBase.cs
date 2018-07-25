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
    public Text textAnswer;
    public Image imgResult;
    public Sprite sprRight;
    public Sprite sprError;

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
                btsNext.button.gameObject.SetActive(isRandom);
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
        if (IsRandom)
        {
            StartExercise();
        }
        else
        {
            CleanQuestion();
        }
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
            StartLightExam();
        }
        else
        {
            CleanQuestion();
        }
    }
    /// <summary>
    /// 点击下一套/下一题
    /// </summary>
    void OnClickNext()
    {
        if (IsLightExam)//灯光考试
        {
            StartLightExam();
        }
        else if (IsRandom)//随机练习
        {
            StartExercise();
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
            if (value != toggleHeadlightSwitch)
            {
                toggleHeadlightSwitch = value;
                LowToHigCount += 1;
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
    /// 考试试题列表
    /// </summary>
    private List<int> examList = new List<int>();
    /// <summary>
    /// 前一道试题
    /// </summary>
    private int prevIndex;
    /// <summary>
    /// 语音播放控件
    /// </summary>
    private AudioObject audioObject;

    /// <summary>
    /// 关闭所有灯光
    /// </summary>
    private void CloseAllLight()
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
    /// <summary>
    /// 清理试题信息
    /// </summary>
    private void CleanQuestion()
    {
        PauseQuestion();
        CloseAllLight();
        textQuestion.text = "";
        textAnswer.text = "";
        imgResult.gameObject.SetActive(false);
    }
    /// <summary>
    /// 停止试题运行
    /// </summary>
    private void PauseQuestion()
    {
        StopAllCoroutines();
        AudioSystemMgr.Instance.StopSoundByAudio(audioObject);
        imgResult.gameObject.SetActive(false);
    }

    /// <summary>
    /// 开始灯光考试
    /// </summary>
    private void StartLightExam()
    {
        PauseQuestion();
        CloseAllLight();
        //生成试题列表
        examList.Add(0);
        examList.Add(1);
        examList.Add(2);
        StartCoroutine(_BeginLightExam());

         }

    IEnumerator _BeginLightExam()
    {
        textQuestion.text = ConfigDataMgr.ExamStartTip;
        textAnswer.text = "";
        audioObject = AudioSystemMgr.Instance.PlaySoundByClip(ResourcesMgr.Instance.GetAudioWithStr(ConfigDataMgr.ExamStartTip));
        yield return new WaitForSeconds(audioObject.playTime);
        audioObject = null;
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < examList.Count; i++)
        {
            QuestionData question = ConfigDataMgr.Instance.GetQuestionByIndex(examList[i]);
            yield return StartCoroutine(BeginQuestion(question));
        }
        yield return StartCoroutine(BeginQuestion(ConfigDataMgr.ExamEnd));
        textAnswer.text = "恭喜你全部操作成功！！！";

    }

    /// <summary>
    /// 开始随机练习
    /// </summary>
    private void StartExercise()
    {
        PauseQuestion();
        CloseAllLight();
        int index = 1;
        QuestionData question = ConfigDataMgr.Instance.GetQuestionByIndex(index);
        StartCoroutine(BeginQuestion(question));
    }

    /// <summary>
    /// Begins the question.
    /// </summary>
    /// <returns>The question.</returns>
    /// <param name="question">Question.</param>
    IEnumerator BeginQuestion(QuestionData question)
    {
        textQuestion.text = question.question;
        textAnswer.text = question.answer;
        textAnswer.gameObject.SetActive(false || IsShowAnswer);
        audioObject = AudioSystemMgr.Instance.PlaySoundByClip(ResourcesMgr.Instance.GetAudioWithStr(question.question));
        yield return new WaitForSeconds(audioObject.playTime);
        audioObject = null;
        LowToHigCount = 0;//防止抢先操作
        yield return new WaitForSeconds(5f);//操作时间

        bool result = true;
        result &= (question.DoubleJumpLamp == DoubleJumpLamp);
        result &= (question.ClearAnceLamp == ClearanceLamp);
        result &= (question.LowBeamLight == LowBeamLight);
        result &= (question.HigBeamLight == HigBeamLight);
        result &= (question.FrontFogLamp == FrontFogLamp);
        result &= (question.RearFogLamp == RearFogLamp);
        result &= (question.LeftIndicator == LeftIndicator);
        result &= (question.RightIndicator == RightIndicator);
        result &= (question.LowToHigLight == (LowToHigCount == 2));

        textAnswer.gameObject.SetActive(true);
        imgResult.gameObject.SetActive(true);
        imgResult.sprite = result ? sprRight : sprError;
        if (result)
        {
            yield return new WaitForSeconds(3.0f);
            imgResult.gameObject.SetActive(false);
        }
        else
        {
            PauseQuestion();
        }
    }
}
