// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/Player Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""cc3c57e1-bf7b-419d-bc8d-2f113d1cc345"",
            ""actions"": [
                {
                    ""name"": ""Z move"",
                    ""type"": ""Value"",
                    ""id"": ""9ba12ccd-4200-4153-a3d0-d363bbc30ad5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Y move"",
                    ""type"": ""Value"",
                    ""id"": ""dd9271c5-b28c-4dc9-a45f-0c2c325fdd9c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""X move"",
                    ""type"": ""Value"",
                    ""id"": ""c5787abb-66e9-4126-9c8c-06efd315d415"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera Input"",
                    ""type"": ""Value"",
                    ""id"": ""9cd3017b-4c3b-41fb-aacc-0b7952c53bcb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera Position"",
                    ""type"": ""Value"",
                    ""id"": ""5dd53664-4fe0-45ed-8f4e-6661d1115cf6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Break Voxel"",
                    ""type"": ""Value"",
                    ""id"": ""13f7a63c-44bd-4391-b430-d8bf05b17bb4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Place Voxel"",
                    ""type"": ""Value"",
                    ""id"": ""6cd13a25-9d54-4699-8720-e8018be77a94"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle UI"",
                    ""type"": ""Button"",
                    ""id"": ""3d8fffce-4915-4846-8c0e-01ed61646d6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switch Item"",
                    ""type"": ""Value"",
                    ""id"": ""2b51a82b-6566-4670-8fbc-23fbd22d4892"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""806aae12-4a51-4fc3-91b7-62223086634d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Z move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c85fbc82-c460-4177-94ca-dc01235badb1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Z move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1a55c023-daa8-49cf-815e-4c4cbe8c4293"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Z move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cae8cccb-8dfd-4a5c-8e76-682da24ce389"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": ""Android"",
                    ""action"": ""Z move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""f9919373-9057-4a5c-8d5d-445d98160abb"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Y move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a32a9a14-ac64-4a68-ab6f-14571d275de2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Y move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d11468f0-4ff0-4692-9165-790f0c1ad43d"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Y move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d0138328-712c-47ee-b757-9ccced10bb6a"",
                    ""path"": ""<Gamepad>/dpad/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Y move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""c591b5c9-6682-4e91-a435-944c22f7fa5e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""X move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0dfe6351-94b5-44bc-96b6-fc10afd48963"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""X move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""00ae1852-7f87-43e7-bc4d-d627d0384407"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""X move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""74b59b53-49af-4f69-bac5-4df8add30683"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""X move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fd99841-7489-44d4-a69c-8aef2e8bbc1e"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Camera Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2323c338-34b2-4c68-b913-f64559cbe51a"",
                    ""path"": ""<VirtualMouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Camera Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2b2a626-1432-4a25-83e5-98cdf223a928"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Break Voxel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a11e14d8-184d-4023-8758-8a1db799830d"",
                    ""path"": ""<VirtualMouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Break Voxel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74ac90bb-ed3a-409b-8b1e-42e9bc5196d2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Place Voxel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b158b5b-5201-40c8-a199-c42459517d26"",
                    ""path"": ""<VirtualMouse>/rightButton"",
                    ""interactions"": ""Tap(duration=0.35)"",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Place Voxel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3da9c97f-abe6-4d51-b7d5-37de75b3b3ff"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Toggle UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a32b754-790d-4b06-b3f7-449afe055302"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Switch Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fddef83b-e5af-46b9-963f-afc508360d35"",
                    ""path"": ""<VirtualMouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Camera Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""d40bfb29-2f66-4bf6-bf1d-95764dee39f5"",
            ""actions"": [
                {
                    ""name"": ""Toggle Player"",
                    ""type"": ""Button"",
                    ""id"": ""96800a20-1a36-4326-b308-58362420ea75"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""846c7ca3-c1d3-4ad3-a83d-d8e5758f4718"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Toggle Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Android"",
            ""bindingGroup"": ""Android"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<VirtualMouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Zmove = m_Player.FindAction("Z move", throwIfNotFound: true);
        m_Player_Ymove = m_Player.FindAction("Y move", throwIfNotFound: true);
        m_Player_Xmove = m_Player.FindAction("X move", throwIfNotFound: true);
        m_Player_CameraInput = m_Player.FindAction("Camera Input", throwIfNotFound: true);
        m_Player_CameraPosition = m_Player.FindAction("Camera Position", throwIfNotFound: true);
        m_Player_BreakVoxel = m_Player.FindAction("Break Voxel", throwIfNotFound: true);
        m_Player_PlaceVoxel = m_Player.FindAction("Place Voxel", throwIfNotFound: true);
        m_Player_ToggleUI = m_Player.FindAction("Toggle UI", throwIfNotFound: true);
        m_Player_SwitchItem = m_Player.FindAction("Switch Item", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_TogglePlayer = m_UI.FindAction("Toggle Player", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Zmove;
    private readonly InputAction m_Player_Ymove;
    private readonly InputAction m_Player_Xmove;
    private readonly InputAction m_Player_CameraInput;
    private readonly InputAction m_Player_CameraPosition;
    private readonly InputAction m_Player_BreakVoxel;
    private readonly InputAction m_Player_PlaceVoxel;
    private readonly InputAction m_Player_ToggleUI;
    private readonly InputAction m_Player_SwitchItem;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Zmove => m_Wrapper.m_Player_Zmove;
        public InputAction @Ymove => m_Wrapper.m_Player_Ymove;
        public InputAction @Xmove => m_Wrapper.m_Player_Xmove;
        public InputAction @CameraInput => m_Wrapper.m_Player_CameraInput;
        public InputAction @CameraPosition => m_Wrapper.m_Player_CameraPosition;
        public InputAction @BreakVoxel => m_Wrapper.m_Player_BreakVoxel;
        public InputAction @PlaceVoxel => m_Wrapper.m_Player_PlaceVoxel;
        public InputAction @ToggleUI => m_Wrapper.m_Player_ToggleUI;
        public InputAction @SwitchItem => m_Wrapper.m_Player_SwitchItem;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Zmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZmove;
                @Zmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZmove;
                @Zmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZmove;
                @Ymove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYmove;
                @Ymove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYmove;
                @Ymove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYmove;
                @Xmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXmove;
                @Xmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXmove;
                @Xmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXmove;
                @CameraInput.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraInput;
                @CameraInput.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraInput;
                @CameraInput.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraInput;
                @CameraPosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraPosition;
                @CameraPosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraPosition;
                @CameraPosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraPosition;
                @BreakVoxel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBreakVoxel;
                @BreakVoxel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBreakVoxel;
                @BreakVoxel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBreakVoxel;
                @PlaceVoxel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlaceVoxel;
                @PlaceVoxel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlaceVoxel;
                @PlaceVoxel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlaceVoxel;
                @ToggleUI.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleUI;
                @ToggleUI.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleUI;
                @ToggleUI.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleUI;
                @SwitchItem.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchItem;
                @SwitchItem.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchItem;
                @SwitchItem.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchItem;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Zmove.started += instance.OnZmove;
                @Zmove.performed += instance.OnZmove;
                @Zmove.canceled += instance.OnZmove;
                @Ymove.started += instance.OnYmove;
                @Ymove.performed += instance.OnYmove;
                @Ymove.canceled += instance.OnYmove;
                @Xmove.started += instance.OnXmove;
                @Xmove.performed += instance.OnXmove;
                @Xmove.canceled += instance.OnXmove;
                @CameraInput.started += instance.OnCameraInput;
                @CameraInput.performed += instance.OnCameraInput;
                @CameraInput.canceled += instance.OnCameraInput;
                @CameraPosition.started += instance.OnCameraPosition;
                @CameraPosition.performed += instance.OnCameraPosition;
                @CameraPosition.canceled += instance.OnCameraPosition;
                @BreakVoxel.started += instance.OnBreakVoxel;
                @BreakVoxel.performed += instance.OnBreakVoxel;
                @BreakVoxel.canceled += instance.OnBreakVoxel;
                @PlaceVoxel.started += instance.OnPlaceVoxel;
                @PlaceVoxel.performed += instance.OnPlaceVoxel;
                @PlaceVoxel.canceled += instance.OnPlaceVoxel;
                @ToggleUI.started += instance.OnToggleUI;
                @ToggleUI.performed += instance.OnToggleUI;
                @ToggleUI.canceled += instance.OnToggleUI;
                @SwitchItem.started += instance.OnSwitchItem;
                @SwitchItem.performed += instance.OnSwitchItem;
                @SwitchItem.canceled += instance.OnSwitchItem;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_TogglePlayer;
    public struct UIActions
    {
        private @PlayerInput m_Wrapper;
        public UIActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @TogglePlayer => m_Wrapper.m_UI_TogglePlayer;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @TogglePlayer.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTogglePlayer;
                @TogglePlayer.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTogglePlayer;
                @TogglePlayer.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTogglePlayer;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TogglePlayer.started += instance.OnTogglePlayer;
                @TogglePlayer.performed += instance.OnTogglePlayer;
                @TogglePlayer.canceled += instance.OnTogglePlayer;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_AndroidSchemeIndex = -1;
    public InputControlScheme AndroidScheme
    {
        get
        {
            if (m_AndroidSchemeIndex == -1) m_AndroidSchemeIndex = asset.FindControlSchemeIndex("Android");
            return asset.controlSchemes[m_AndroidSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnZmove(InputAction.CallbackContext context);
        void OnYmove(InputAction.CallbackContext context);
        void OnXmove(InputAction.CallbackContext context);
        void OnCameraInput(InputAction.CallbackContext context);
        void OnCameraPosition(InputAction.CallbackContext context);
        void OnBreakVoxel(InputAction.CallbackContext context);
        void OnPlaceVoxel(InputAction.CallbackContext context);
        void OnToggleUI(InputAction.CallbackContext context);
        void OnSwitchItem(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnTogglePlayer(InputAction.CallbackContext context);
    }
}
