using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExamWindowAiLiShe : UIExamWindowBase
{
    public override bool ClearanceSwitch
    {
        set
        {
            if (ClearanceSwitch != value)
            {
                base.ClearanceSwitch = value;
                if (value)
                {
                    //knobSwitch.SetLevel(1);
                }
                else if (!HeadlightSwitch)
                {
                    //knobSwitch.SetLevel(0);
                }
            }
        }
    }
    public override bool HeadlightSwitch
    {
        set
        {
            if (HeadlightSwitch != value)
            {
                base.HeadlightSwitch = value;
                if (value)
                {
                    //knobSwitch.SetLevel(2);
                }
                else if (!ClearanceSwitch)
                {
                    //knobSwitch.SetLevel(0);
                }
            }
        }
    }
    public override bool FrontFogSwitch
    {
        set
        {
            if (FrontFogSwitch != value)
            {
                base.FrontFogSwitch = value;
                if (!FrontFogSwitch && !RearFogSwitch)
                {
                    //knobSwitch.IsTriggerOn = false;
                }
            }
        }
    }
    public override bool RearFogSwitch
    {
        set
        {
            if (RearFogSwitch != value)
            {
                base.RearFogSwitch = value;
                if (!FrontFogSwitch && !RearFogSwitch)
                {
                    //knobSwitch.IsTriggerOn = false;
                }
            }
        }
    }
    public override bool LeftIndicatorSwitch
    {
        set
        {
            if (LeftIndicatorSwitch != value)
            {
                base.LeftIndicatorSwitch = value;
                if (value)
                {
                    //imgControlRod.transform.localEulerAngles = new Vector3(0, 0, 10);
                }
                else if (!RightIndicatorSwitch)
                {
                    //imgControlRod.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
    }
    public override bool RightIndicatorSwitch
    {
        set
        {
            if (RightIndicatorSwitch != value)
            {
                base.RightIndicatorSwitch = value;
                if (value)
                {
                    //imgControlRod.transform.localEulerAngles = new Vector3(0, 0, -10);
                }
                else if (!LeftIndicatorSwitch)
                {
                    //imgControlRod.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
    }
    public override bool DoubleJumpSwitch
    {
        set
        {
            if (DoubleJumpSwitch != value)
            {
                base.DoubleJumpSwitch = value;
                //(btsDoubleJump.button.targetGraphic as Image).sprite = value ? btsDoubleJump.sprSelect : btsDoubleJump.sprNormal;
            }
        }
    }
    public override bool FarHeadlightSwitch
    {
        set
        {
            if (FarHeadlightSwitch != value)
            {
                base.FarHeadlightSwitch = value;
                //(btsCantrolForward.button.targetGraphic as Image).sprite = (value && !ToggleHeadlightSwitch) ? btsCantrolForward.sprSelect : btsCantrolForward.sprNormal;
                //(btsControlNormal.button.targetGraphic as Image).sprite = (!value && !ToggleHeadlightSwitch) ? btsControlNormal.sprSelect : btsControlNormal.sprNormal;
                //imgControlRod.sprite = value ? ((!ToggleHeadlightSwitch) ? sprControlForward : sprControlBackward) : sprControlNormal;
            }
        }
    }
    public override bool ToggleHeadlightSwitch
    {
        set
        {
            if (ToggleHeadlightSwitch != value)
            {
                base.ToggleHeadlightSwitch = value;
                //(btsCantrolForward.button.targetGraphic as Image).sprite = value ? btsCantrolForward.sprNormal : FarHeadlightSwitch ? btsCantrolForward.sprSelect : btsCantrolForward.sprNormal;
                //(btsControlNormal.button.targetGraphic as Image).sprite = value ? btsControlNormal.sprNormal : FarHeadlightSwitch ? btsControlNormal.sprNormal : btsControlNormal.sprSelect;
                //(btsControlBackward.button.targetGraphic as Image).sprite = value ? btsControlBackward.sprSelect : btsControlBackward.sprNormal;
                //imgControlRod.sprite = value ? sprControlBackward : FarHeadlightSwitch ? sprControlForward : sprControlNormal;
            }
        }
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }
}
