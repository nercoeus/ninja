using UnityEngine;
using UnityEngine.EventSystems;

public class MapTile : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("print");
        NotificationCenter.Instance.PushEvent(NotificationType.Operate_MapPosition, transform.position);
    }
}
