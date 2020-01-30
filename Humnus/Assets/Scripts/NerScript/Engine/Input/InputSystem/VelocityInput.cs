// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/NerScript/Engine/Input/InputSystem/VelocityInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @VelocityInput : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @VelocityInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""VelocityInput"",
    ""maps"": [
        {
            ""name"": ""Normal"",
            ""id"": ""35498367-d65b-4573-9a22-441c971bf890"",
            ""actions"": [
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""ce980257-0dd0-43b7-a089-ef6cd43e9c61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""f49afcc3-4ef4-4294-b41a-e0db8e997ba4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""65c940e1-23a2-47be-8cbc-7a27b47009fc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""687da16f-ee6e-4d88-8ae7-0adc6dc415f4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0ffd66a-12ac-4326-be5d-0ddc85534460"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6923961a-fc7b-46f2-ae02-1baf71ee7080"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8b20394-8932-4b75-83af-69b1ffa907f3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b0a5b59-93a0-4a91-a2f2-386ff4b518a9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02be5f22-0434-4062-a58f-b6d677bd4d29"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9ea486e-8ae3-4d9e-8d0a-f2f9b163a123"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58af0dd8-2a1e-4e56-832c-645053327a09"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c34ee82f-f9c2-4502-b779-f19ec6830bfd"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Normal
        m_Normal = asset.FindActionMap("Normal", throwIfNotFound: true);
        m_Normal_Up = m_Normal.FindAction("Up", throwIfNotFound: true);
        m_Normal_Left = m_Normal.FindAction("Left", throwIfNotFound: true);
        m_Normal_Down = m_Normal.FindAction("Down", throwIfNotFound: true);
        m_Normal_Right = m_Normal.FindAction("Right", throwIfNotFound: true);
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

    // Normal
    private readonly InputActionMap m_Normal;
    private INormalActions m_NormalActionsCallbackInterface;
    private readonly InputAction m_Normal_Up;
    private readonly InputAction m_Normal_Left;
    private readonly InputAction m_Normal_Down;
    private readonly InputAction m_Normal_Right;
    public struct NormalActions
    {
        private @VelocityInput m_Wrapper;
        public NormalActions(@VelocityInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Up => m_Wrapper.m_Normal_Up;
        public InputAction @Left => m_Wrapper.m_Normal_Left;
        public InputAction @Down => m_Wrapper.m_Normal_Down;
        public InputAction @Right => m_Wrapper.m_Normal_Right;
        public InputActionMap Get() { return m_Wrapper.m_Normal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NormalActions set) { return set.Get(); }
        public void SetCallbacks(INormalActions instance)
        {
            if (m_Wrapper.m_NormalActionsCallbackInterface != null)
            {
                @Up.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnUp;
                @Left.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnLeft;
                @Down.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnDown;
                @Down.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnDown;
                @Down.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnDown;
                @Right.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnRight;
                @Right.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnRight;
                @Right.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnRight;
            }
            m_Wrapper.m_NormalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
                @Down.started += instance.OnDown;
                @Down.performed += instance.OnDown;
                @Down.canceled += instance.OnDown;
                @Right.started += instance.OnRight;
                @Right.performed += instance.OnRight;
                @Right.canceled += instance.OnRight;
            }
        }
    }
    public NormalActions @Normal => new NormalActions(this);
    public interface INormalActions
    {
        void OnUp(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
    }
}
