using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class RotateImage : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        //拖拽旋转图片
        SetDraggedRotation(eventData);
    }

    private void SetDraggedRotation(PointerEventData eventData)
    {
        Vector2 curScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, transform.position);
        Vector2 directionTo = curScreenPosition - eventData.position;
        Vector2 directionFrom = directionTo - eventData.delta;
        Vector3 angle = transform.localEulerAngles;
        if (angle.z >= -50)
        {
            this.transform.rotation *= Quaternion.FromToRotation(directionTo, directionFrom);
        }
        angle.z = Mathf.Clamp(angle.z, -49, 9);

        Debug.Log(angle);
    }
}