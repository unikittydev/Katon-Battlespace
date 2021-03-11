using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenCameraDelta : OnScreenControl, IPointerUpHandler, IDragHandler
{
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_controlPath;

    protected override string controlPathInternal
    {
        get => m_controlPath;
        set => m_controlPath = value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        SendValueToControl(eventData.delta);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SendValueToControl(Vector2.zero);
    }
}
