using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExamWindowDaZhong : UIExamWindowBase
{

    [Space(20)]
    public KnobSwitch knobSwitch;       //灯光控制旋钮 [示廓灯、近光灯、雾灯]

    public Button btnControlRigth;      //右转向
    public Button btnControlClose;      //关闭转向
    public Button btnControlLeft;       //左转向

    public ButtonState btsCantrolForward;   //远光灯
    public ButtonState btsControlNormal;    //近光灯
    public ButtonState btsControlBackward;  //远近切换

    public ButtonState btsDoubleJump;       //双闪灯

    public Image imgControlRod;         //灯光控制杆

    public Sprite sprControlForward;    //往前--变小
    public Sprite sprControlNormal;     //默认
    public Sprite sprControlBackward;   //往后--变大


    public override void OnCreate()
    {
        base.OnCreate();

        knobSwitch.onChangeLevel = (value) =>
        {
            CarLightController carLightController = FindObjectOfType<CarLightController>();
            carLightController.SetLight(value == 1);
        };
    }




}
