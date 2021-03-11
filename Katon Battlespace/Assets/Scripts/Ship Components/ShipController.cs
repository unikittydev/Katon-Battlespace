using Game.Modules;
using Game.Modules.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    private ShipInput input;

    private void Awake()
    {
        input = new ShipInput();

        var bindingGroup = Application.isMobilePlatform ? input.AndroidScheme.bindingGroup : input.KeyboardandMouseScheme.bindingGroup;
        input.bindingMask = InputBinding.MaskByGroup(bindingGroup);

        input.Player.ToggleUI.performed += _ =>
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            input.Player.Disable();
            input.UI.Enable();
        };

        input.UI.TogglePlayer.performed += _ =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            input.Player.Enable();
            input.UI.Disable();
        };
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OnDestroy()
    {
        input.Dispose();
    }

    public void InitInput(IActionData data, IActionModule actionModule)
    {
        InputAction action = input.asset.FindAction(data.actionName);
        action.performed += actionModule.OnPerformInput;
        action.canceled += actionModule.OnCancelInput;
    }
}
