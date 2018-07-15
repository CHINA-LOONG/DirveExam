using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public Image imgProgress;

    [Range(0.0f, 1.0f)]
    private float _value;

    public float Value
    {
        get { return imgProgress.fillAmount; }
        set { imgProgress.fillAmount = value; }
    }
}
