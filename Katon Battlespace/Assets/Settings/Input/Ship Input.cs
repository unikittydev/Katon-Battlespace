// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/Ship Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ShipInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ShipInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Ship Input"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b9ba1d23-114b-42d0-9a32-6fce90c121a3"",
            ""actions"": [
                {
                    ""name"": ""Zpos move"",
                    ""type"": ""Value"",
                    ""id"": ""b8e91733-0ee9-4444-8edb-a8988352b395"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zneg move"",
                    ""type"": ""Value"",
                    ""id"": ""47c564cb-ca1e-4931-9f4f-d62ee073232d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ypos move"",
                    ""type"": ""Value"",
                    ""id"": ""bee9d103-a72b-4ca3-820f-696591f29455"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Yneg move"",
                    ""type"": ""Value"",
                    ""id"": ""bcf4dc66-0429-4a9a-a603-4b0f2bd95fcb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Xpos move"",
                    ""type"": ""Value"",
                    ""id"": ""1b547ff7-c281-4666-9823-dc6af8326e84"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Xneg move"",
                    ""type"": ""Value"",
                    ""id"": ""d4c0f98b-2fb9-4b36-9b27-9f1cf455b72f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Energy produce toggle"",
                    ""type"": ""Button"",
                    ""id"": ""90a94c9d-291b-4597-af56-727c8bebe75b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Z rotate"",
                    ""type"": ""Value"",
                    ""id"": ""89487af4-8544-48be-adc4-585b1c981f6a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Y rotate"",
                    ""type"": ""Value"",
                    ""id"": ""a5c4a61c-faf8-4b01-bd78-b95a47771287"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""X rotate"",
                    ""type"": ""Value"",
                    ""id"": ""1abaf982-e51b-487e-95db-c3f1972e2290"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera move"",
                    ""type"": ""Value"",
                    ""id"": ""2f7c0d0c-ba26-4310-a2a2-50bb02af476c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Value"",
                    ""id"": ""b996eb8b-5d27-475c-85f6-92141e8f21a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle UI"",
                    ""type"": ""Button"",
                    ""id"": ""d9e9a26c-bc06-4ee9-9226-e29011620915"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""03747ea7-df16-471f-adba-dd95cc5202e5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Zpos move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c975423-3589-4ddd-ba7d-1b2b5055dacb"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Zpos move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4327983d-7dbe-48f0-b3a1-3d990adb25c4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Zneg move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57a617d6-0cdf-490b-87f4-4104c1c896da"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Zneg move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""013df64d-93ed-4bcd-a5ae-b7f13561f2bc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Ypos move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4caf47c6-e7fe-4d6a-8770-a4ad993bbdae"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Yneg move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""994b9f77-b9ac-4494-8691-c3a36b797033"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Xpos move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f37cbb01-e4da-4b8f-8107-98918b8e8e60"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Xpos move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94f9b184-9d1f-4f74-8073-1b18b16ae26d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Xneg move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a966f3c-3ad1-4a59-a072-7fece6b6a1d4"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Xneg move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""055db7b8-23c9-4d88-a568-8628ab53e9cd"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Energy produce toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ea3ef8fa-adb2-441c-ad11-2ba12f970282"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Z rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0f4b79d7-61af-4a41-9564-c591510f059a"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Z rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0d0a11d8-b088-4f8a-9c79-7f6b1ab189c2"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Z rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""86374c50-45bd-4158-934b-5c9b6748cc76"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": ""Android"",
                    ""action"": ""Z rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""001c44fe-fbb0-46f0-8252-6bcc2cfeba6d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Y rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e433389f-7477-40fa-bca7-8c3a79661f47"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Y rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3cc726e7-cfe1-4549-8a72-45adc9a8c7bd"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Y rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""9f341aa4-34ca-4e4f-9135-8f063b735543"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""X rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0148ea8e-33e2-4416-9667-a942dc26feac"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""X rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""68e33a8a-2c6b-49d3-9f15-2d7280d53382"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""X rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""56cfa661-80cc-43b3-afd8-ed2852b9981e"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": ""Android"",
                    ""action"": ""X rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e4f6836-a326-4b2a-ab35-6ad7fe4e54a2"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Camera move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""864424fe-e98a-4968-a5ad-d749d352d0c7"",
                    ""path"": ""<VirtualMouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Camera move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3779b621-3315-4633-a947-3a8b8fd6233d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b53b87aa-09d1-4590-9065-a2a4e8d84b21"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Android"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01ebd41b-c05f-42dd-853b-40c0041f1522"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Toggle UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""804c4f55-7c13-4742-8d80-62c1a6e612dd"",
            ""actions"": [
                {
                    ""name"": ""Toggle Player"",
                    ""type"": ""Button"",
                    ""id"": ""8f580093-26b5-4a14-b846-691f9b60f7ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""add56080-1f2e-48c8-9b6d-1562d0f5669e"",
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
                    ""devicePath"": ""<AndroidGamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Zposmove = m_Player.FindAction("Zpos move", throwIfNotFound: true);
        m_Player_Znegmove = m_Player.FindAction("Zneg move", throwIfNotFound: true);
        m_Player_Yposmove = m_Player.FindAction("Ypos move", throwIfNotFound: true);
        m_Player_Ynegmove = m_Player.FindAction("Yneg move", throwIfNotFound: true);
        m_Player_Xposmove = m_Player.FindAction("Xpos move", throwIfNotFound: true);
        m_Player_Xnegmove = m_Player.FindAction("Xneg move", throwIfNotFound: true);
        m_Player_Energyproducetoggle = m_Player.FindAction("Energy produce toggle", throwIfNotFound: true);
        m_Player_Zrotate = m_Player.FindAction("Z rotate", throwIfNotFound: true);
        m_Player_Yrotate = m_Player.FindAction("Y rotate", throwIfNotFound: true);
        m_Player_Xrotate = m_Player.FindAction("X rotate", throwIfNotFound: true);
        m_Player_Cameramove = m_Player.FindAction("Camera move", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_ToggleUI = m_Player.FindAction("Toggle UI", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Zposmove;
    private readonly InputAction m_Player_Znegmove;
    private readonly InputAction m_Player_Yposmove;
    private readonly InputAction m_Player_Ynegmove;
    private readonly InputAction m_Player_Xposmove;
    private readonly InputAction m_Player_Xnegmove;
    private readonly InputAction m_Player_Energyproducetoggle;
    private readonly InputAction m_Player_Zrotate;
    private readonly InputAction m_Player_Yrotate;
    private readonly InputAction m_Player_Xrotate;
    private readonly InputAction m_Player_Cameramove;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_ToggleUI;
    public struct PlayerActions
    {
        private @ShipInput m_Wrapper;
        public PlayerActions(@ShipInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Zposmove => m_Wrapper.m_Player_Zposmove;
        public InputAction @Znegmove => m_Wrapper.m_Player_Znegmove;
        public InputAction @Yposmove => m_Wrapper.m_Player_Yposmove;
        public InputAction @Ynegmove => m_Wrapper.m_Player_Ynegmove;
        public InputAction @Xposmove => m_Wrapper.m_Player_Xposmove;
        public InputAction @Xnegmove => m_Wrapper.m_Player_Xnegmove;
        public InputAction @Energyproducetoggle => m_Wrapper.m_Player_Energyproducetoggle;
        public InputAction @Zrotate => m_Wrapper.m_Player_Zrotate;
        public InputAction @Yrotate => m_Wrapper.m_Player_Yrotate;
        public InputAction @Xrotate => m_Wrapper.m_Player_Xrotate;
        public InputAction @Cameramove => m_Wrapper.m_Player_Cameramove;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @ToggleUI => m_Wrapper.m_Player_ToggleUI;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Zposmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZposmove;
                @Zposmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZposmove;
                @Zposmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZposmove;
                @Znegmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZnegmove;
                @Znegmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZnegmove;
                @Znegmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZnegmove;
                @Yposmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYposmove;
                @Yposmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYposmove;
                @Yposmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYposmove;
                @Ynegmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYnegmove;
                @Ynegmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYnegmove;
                @Ynegmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYnegmove;
                @Xposmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXposmove;
                @Xposmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXposmove;
                @Xposmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXposmove;
                @Xnegmove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXnegmove;
                @Xnegmove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXnegmove;
                @Xnegmove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXnegmove;
                @Energyproducetoggle.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnergyproducetoggle;
                @Energyproducetoggle.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnergyproducetoggle;
                @Energyproducetoggle.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnergyproducetoggle;
                @Zrotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZrotate;
                @Zrotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZrotate;
                @Zrotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZrotate;
                @Yrotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYrotate;
                @Yrotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYrotate;
                @Yrotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnYrotate;
                @Xrotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXrotate;
                @Xrotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXrotate;
                @Xrotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXrotate;
                @Cameramove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameramove;
                @Cameramove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameramove;
                @Cameramove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameramove;
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @ToggleUI.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleUI;
                @ToggleUI.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleUI;
                @ToggleUI.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleUI;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Zposmove.started += instance.OnZposmove;
                @Zposmove.performed += instance.OnZposmove;
                @Zposmove.canceled += instance.OnZposmove;
                @Znegmove.started += instance.OnZnegmove;
                @Znegmove.performed += instance.OnZnegmove;
                @Znegmove.canceled += instance.OnZnegmove;
                @Yposmove.started += instance.OnYposmove;
                @Yposmove.performed += instance.OnYposmove;
                @Yposmove.canceled += instance.OnYposmove;
                @Ynegmove.started += instance.OnYnegmove;
                @Ynegmove.performed += instance.OnYnegmove;
                @Ynegmove.canceled += instance.OnYnegmove;
                @Xposmove.started += instance.OnXposmove;
                @Xposmove.performed += instance.OnXposmove;
                @Xposmove.canceled += instance.OnXposmove;
                @Xnegmove.started += instance.OnXnegmove;
                @Xnegmove.performed += instance.OnXnegmove;
                @Xnegmove.canceled += instance.OnXnegmove;
                @Energyproducetoggle.started += instance.OnEnergyproducetoggle;
                @Energyproducetoggle.performed += instance.OnEnergyproducetoggle;
                @Energyproducetoggle.canceled += instance.OnEnergyproducetoggle;
                @Zrotate.started += instance.OnZrotate;
                @Zrotate.performed += instance.OnZrotate;
                @Zrotate.canceled += instance.OnZrotate;
                @Yrotate.started += instance.OnYrotate;
                @Yrotate.performed += instance.OnYrotate;
                @Yrotate.canceled += instance.OnYrotate;
                @Xrotate.started += instance.OnXrotate;
                @Xrotate.performed += instance.OnXrotate;
                @Xrotate.canceled += instance.OnXrotate;
                @Cameramove.started += instance.OnCameramove;
                @Cameramove.performed += instance.OnCameramove;
                @Cameramove.canceled += instance.OnCameramove;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @ToggleUI.started += instance.OnToggleUI;
                @ToggleUI.performed += instance.OnToggleUI;
                @ToggleUI.canceled += instance.OnToggleUI;
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
        private @ShipInput m_Wrapper;
        public UIActions(@ShipInput wrapper) { m_Wrapper = wrapper; }
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
        void OnZposmove(InputAction.CallbackContext context);
        void OnZnegmove(InputAction.CallbackContext context);
        void OnYposmove(InputAction.CallbackContext context);
        void OnYnegmove(InputAction.CallbackContext context);
        void OnXposmove(InputAction.CallbackContext context);
        void OnXnegmove(InputAction.CallbackContext context);
        void OnEnergyproducetoggle(InputAction.CallbackContext context);
        void OnZrotate(InputAction.CallbackContext context);
        void OnYrotate(InputAction.CallbackContext context);
        void OnXrotate(InputAction.CallbackContext context);
        void OnCameramove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnToggleUI(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnTogglePlayer(InputAction.CallbackContext context);
    }
}
