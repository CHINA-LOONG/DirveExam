using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UIEventListener : MonoBehaviour, IPointerClickHandler,
                                                  IPointerDownHandler,
                                                  IPointerUpHandler,
                                                  IPointerExitHandler,
                                                  IPointerEnterHandler,
                                                  IBeginDragHandler,
                                                  IDragHandler,
                                                  IEndDragHandler,
                                                  IInitializePotentialDragHandler
                                                  

{
    public delegate void VoidDelegate(GameObject go);
    public delegate void VoidDelegate_Vector2(GameObject go, PointerEventData pos);
    public delegate void VoidDelegate_Bool_Vector2(GameObject go,bool state, Vector2 pos);

    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onUp;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onPressEnter;
    public VoidDelegate onPressExit;
    
    public VoidDelegate_Vector2 onDragBegin;
    public VoidDelegate_Vector2 onDrag;
    public VoidDelegate_Vector2 onDragEnd;

    public VoidDelegate_Bool_Vector2 onPress;

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (needPressExit) return;
        if (onClick != null && !press && !isDrag) onClick(gameObject);
    }


    void Update()
    {
        if (onPressEnter != null)
        {
            if (pressDown && !enterPress)
            {
                if (Time.time - pressTime > 0.5f)
                {
                    onPressEnter(gameObject);
                    press = true;
                    enterPress = true;
                    needPressExit = true;
                }
            }
            if (!pressDown && press)
            {
                press = false;
                enterPress = false;
                if (onPressExit != null) onPressExit(gameObject);
            }
        }
    }

    #region LongPress_used_member

    private float pressTime;
    private bool pressDown = false;
    private bool press = false;
    private bool enterPress = false;
    private bool needPressExit = false;
    private bool isDrag = false;

    IEnumerator ExitPress()
    {
        yield return 2;
        needPressExit = false;
    }

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
        if (onPress != null) onPress(gameObject, true, eventData.position);

        pressDown = true;
        pressTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
        if (onPress != null) onPress(gameObject, false, eventData.position);

        pressDown = false;
        if (needPressExit)
        {
            StartCoroutine(ExitPress());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
        {
            onExit(gameObject);
        }
        pressDown = false;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
        {
            onEnter(gameObject);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject, eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        if (onDragBegin != null) onDragBegin(gameObject, eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (onDragEnd != null) onDragEnd(gameObject, eventData);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {

    }
}
