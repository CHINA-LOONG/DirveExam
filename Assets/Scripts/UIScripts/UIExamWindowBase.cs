using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExamWindowBase : UIWindow
{
    [System.Serializable]
    public class ButtonState
    {
        public Image image;
        public Sprite sprNormal;
        public Sprite sprSelect;
    }

    public ButtonState btsVideo;    //视频讲解
    public ButtonState btsRandom;   //随机题目
    public ButtonState btsRules;    //新规内容
    public ButtonState btsLightExam;//灯光考试

    //public Button btnNext;          //下一题
    public Button btnHelp;          //内容提示


    public Text textQuestion;
    public Image imgResul;
    public Text textAnswer;
}
