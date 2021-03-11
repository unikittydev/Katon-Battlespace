using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenMouseClick : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    [InputControl(layout = "Single")]
    [SerializeField]
    private string m_controlPath;

    protected override string controlPathInternal
    {
        get => m_controlPath;
        set => m_controlPath = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        SendValueToControl(1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        SendValueToControl(0f);
    }
}