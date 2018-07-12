using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoginWindow : UIWindow
{
    public enum State{
        None,
        Normal,
        Login,
        Update
    }

    protected override void BindListener()
    {
        base.BindListener();
    }
    protected override void UnBindListener()
    {
        base.UnBindListener();
    }

}
