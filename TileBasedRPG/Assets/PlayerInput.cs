// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInput.inputactions'

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
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Battle"",
            ""id"": ""86b2917b-548b-4ef9-9459-85099715f171"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""947de835-fffc-4f03-80d2-0926dc1266ba"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Accept"",
                    ""type"": ""Button"",
                    ""id"": ""d6eb333f-a005-497f-969c-d71ef18db28e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""cf8822c4-36dc-4f82-b6d4-fb90406fe3b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""End Turn"",
                    ""type"": ""Button"",
                    ""id"": ""15ac76ec-3969-4b4f-a570-b1bce3e64a99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Button East"",
                    ""type"": ""Button"",
                    ""id"": ""c2b9c5e3-d62a-42b8-8192-e4f1000295c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Swap"",
                    ""type"": ""Value"",
                    ""id"": ""6fc846df-adcb-4018-92ab-7fb2dba59ff0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""042c1d5c-7109-4732-ab0f-4f088a764393"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39d6ef42-e51b-4832-90e5-061a3a437688"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b8a996c-546e-41ad-8f89-e8ae48f9854b"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""411f0a54-73d2-4d16-9844-f9b57dd8075d"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c82dd38e-4f57-4c0b-ab9f-34fead5991c5"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba632304-6e65-4dcc-a56b-9e7de8ccc27b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""e04c5a26-37a0-4fec-a131-82380f153b84"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""891d8d5f-21fb-4274-8f3b-97d93fbcfea8"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""36a3f61f-cb4b-41f5-9afc-c0770b0f81aa"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bfa9c804-0654-497b-b238-ad7f2eae0b48"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f8a75046-e305-47e7-a67a-10a429ee8c25"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""86b920db-603a-4d4a-835b-9407520582de"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""End Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec8ea8b6-a956-4fd2-8793-b6691f8b3705"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""End Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5445fe1-8a93-4984-8307-fae55c72b99b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""2ae41f00-5f9c-4fb3-9b26-16e7935074a3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a46d0ed9-8a54-4ec2-8014-bda251c1c233"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9160bc6a-5a2c-437e-91bd-9a00de5cebf6"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Battle
        m_Battle = asset.FindActionMap("Battle", throwIfNotFound: true);
        m_Battle_Movement = m_Battle.FindAction("Movement", throwIfNotFound: true);
        m_Battle_Accept = m_Battle.FindAction("Accept", throwIfNotFound: true);
        m_Battle_Cancel = m_Battle.FindAction("Cancel", throwIfNotFound: true);
        m_Battle_EndTurn = m_Battle.FindAction("End Turn", throwIfNotFound: true);
        m_Battle_ButtonEast = m_Battle.FindAction("Button East", throwIfNotFound: true);
        m_Battle_Swap = m_Battle.FindAction("Swap", throwIfNotFound: true);
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

    // Battle
    private readonly InputActionMap m_Battle;
    private IBattleActions m_BattleActionsCallbackInterface;
    private readonly InputAction m_Battle_Movement;
    private readonly InputAction m_Battle_Accept;
    private readonly InputAction m_Battle_Cancel;
    private readonly InputAction m_Battle_EndTurn;
    private readonly InputAction m_Battle_ButtonEast;
    private readonly InputAction m_Battle_Swap;
    public struct BattleActions
    {
        private @PlayerInput m_Wrapper;
        public BattleActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Battle_Movement;
        public InputAction @Accept => m_Wrapper.m_Battle_Accept;
        public InputAction @Cancel => m_Wrapper.m_Battle_Cancel;
        public InputAction @EndTurn => m_Wrapper.m_Battle_EndTurn;
        public InputAction @ButtonEast => m_Wrapper.m_Battle_ButtonEast;
        public InputAction @Swap => m_Wrapper.m_Battle_Swap;
        public InputActionMap Get() { return m_Wrapper.m_Battle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActions instance)
        {
            if (m_Wrapper.m_BattleActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnMovement;
                @Accept.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnAccept;
                @Accept.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnAccept;
                @Accept.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnAccept;
                @Cancel.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnCancel;
                @EndTurn.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnEndTurn;
                @EndTurn.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnEndTurn;
                @EndTurn.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnEndTurn;
                @ButtonEast.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnButtonEast;
                @ButtonEast.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnButtonEast;
                @ButtonEast.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnButtonEast;
                @Swap.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnSwap;
                @Swap.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnSwap;
                @Swap.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnSwap;
            }
            m_Wrapper.m_BattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Accept.started += instance.OnAccept;
                @Accept.performed += instance.OnAccept;
                @Accept.canceled += instance.OnAccept;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @EndTurn.started += instance.OnEndTurn;
                @EndTurn.performed += instance.OnEndTurn;
                @EndTurn.canceled += instance.OnEndTurn;
                @ButtonEast.started += instance.OnButtonEast;
                @ButtonEast.performed += instance.OnButtonEast;
                @ButtonEast.canceled += instance.OnButtonEast;
                @Swap.started += instance.OnSwap;
                @Swap.performed += instance.OnSwap;
                @Swap.canceled += instance.OnSwap;
            }
        }
    }
    public BattleActions @Battle => new BattleActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IBattleActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAccept(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnEndTurn(InputAction.CallbackContext context);
        void OnButtonEast(InputAction.CallbackContext context);
        void OnSwap(InputAction.CallbackContext context);
    }
}
