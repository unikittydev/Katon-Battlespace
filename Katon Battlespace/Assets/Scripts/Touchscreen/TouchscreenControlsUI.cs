using UnityEngine;

public class TouchscreenControlsUI : MonoBehaviour
{
    private bool shouldSetActive;

    private void Awake()
    {
        shouldSetActive = Application.isMobilePlatform && !Application.isEditor;
        TrySetActive();
    }

    public void TrySetActive() => gameObject.SetActive(shouldSetActive);
}
