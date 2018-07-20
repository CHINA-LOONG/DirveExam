using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KnobSwitch1 : MonoBehaviour
{
    public Image knobTrigger;

    public Button btnLevel_0;
    public Button btnLevel_1;
    public Button btnLevel_2;

    public Button btnSwitch;

    public Callback onClick;
    public Callback<int> onChangeLevel;
    public Callback<bool> onChangeSwitch;

    [System.Serializable]
    public class Level
    {
        public bool isClockwise = true;
        [Range(0, 360)]
        public float levelAngle;
        [Range(0, 360)]
        public float minAngle;
        [Range(0, 360)]
        public float maxAngle;

        public float LevelAngle { get { return CalculationAngle(levelAngle); } }
        public float MinAngle { get { return CalculationAngle(minAngle); } }
        public float MaxAngle { get { return CalculationAngle(maxAngle); } }
        float CalculationAngle(float angle)
        {
            if (isClockwise)
            {
                if (angle <= 0)
                {
                    return 360 - angle;
                }
                else
                {
                    return (360 - angle) % 360;
                }
            }
            else
            {
                return angle % 360;
            }
        }
    }

    public List<Level> levels = new List<Level>();

    private int curLevelIdx;
    public int CurLevelIdx
    {
        get{return curLevelIdx; }
        set
        {
            if (curLevelIdx != value)
            {
                //设置开关要在设置级别前
                if (value == 0 && IsTriggerOn)
                {
                    IsTriggerOn = false;
                }
                curLevelIdx = value;
                SetLevel(curLevelIdx);
            }
        }
    }
    private bool isTriggerOn;
    public bool IsTriggerOn
    {
        get  {  return isTriggerOn;  } 
        set
        {
            if (isTriggerOn != value && CurLevelIdx != 0)
            {
                isTriggerOn = value;

                knobTrigger.transform.localScale = Vector3.one * (isTriggerOn ? 0.8f : 1f);
                if (onChangeSwitch != null)
                {
                    onChangeSwitch(value);
                }
            }
        }
    }


    void Start()
    {
        UIEventListener.Get(knobTrigger.gameObject).onDragBegin += OnTriggerDragBegin;
        UIEventListener.Get(knobTrigger.gameObject).onDrag += OnTriggerDrag;

        btnLevel_0.onClick.AddListener(OnClickLevel0);
        btnLevel_1.onClick.AddListener(OnClickLevel1);
        btnLevel_2.onClick.AddListener(OnClickLevel2);

        btnSwitch.onClick.AddListener(OnClickSwitch);
    }

    void OnClickLevel0()
    {
        CurLevelIdx = 0;
    }
    void OnClickLevel1()
    {
        CurLevelIdx = 1;
    }
    void OnClickLevel2()
    {
        CurLevelIdx = 2;
    }

    void OnClickSwitch()
    {
        IsTriggerOn = !IsTriggerOn;
    }

    public void SetLevel(int index)
    {
        Level level = levels[index];
        if (onChangeLevel != null)
        {
            onChangeLevel(index);
        }
        knobTrigger.transform.localEulerAngles = new Vector3(0, 0, level.LevelAngle);
    }
    
    Vector2 beginPosition;


    void OnTriggerDragBegin(GameObject go, PointerEventData eventData)
    {
        beginPosition = eventData.pressPosition;
    }
    void OnTriggerDrag(GameObject go, PointerEventData eventData)
    {
        Vector2 curScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, transform.position);
        Vector2 directionTo = curScreenPosition - eventData.position;
        Vector2 directionFrom = curScreenPosition - beginPosition;

        float angle = NormalizationAngle(go.transform.localEulerAngles.z - Quaternion.FromToRotation(directionTo, directionFrom).eulerAngles.z);

        for (int i = 0; i < levels.Count; i++)
        {
            Level tempLevel = levels[i];
            if (curLevelIdx != i && angle > tempLevel.MaxAngle && angle <= tempLevel.MinAngle)
            {
                CurLevelIdx = i;
            }
        }
    }
    
    private float NormalizationAngle(float angle)
    {
        while (angle<0)
        {
            angle += 360;
        }
        return angle %= 360;
    }
}
