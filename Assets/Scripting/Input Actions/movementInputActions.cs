//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripting/Input Actions/movementInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MovementInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MovementInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""movementInputActions"",
    ""maps"": [
        {
            ""name"": ""playerMovment"",
            ""id"": ""4e3115ac-b80c-4e4e-b420-ab91660a8327"",
            ""actions"": [
                {
                    ""name"": ""Horizontal Movement"",
                    ""type"": ""Value"",
                    ""id"": ""8ba277d1-f1fe-46c3-ad8a-9f2096e551a5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""263bd393-675d-493f-9071-230ec60d764a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""f017f204-dcbe-4bbc-94e0-11a3046762b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""f4b8b0ac-9cee-45f8-a034-d677016b0244"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slam"",
                    ""type"": ""Button"",
                    ""id"": ""19b8547b-3277-45e7-9f72-c21da7b50b09"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FlipGravity"",
                    ""type"": ""Button"",
                    ""id"": ""4e97964e-486b-4a7c-865b-0301121522b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""6d4de2cb-c3dc-4f46-ba24-be239424aecf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""77085246-9746-4c47-9914-4a7b02b776a3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0201be99-4a3b-4c1d-a505-4b263b33cfca"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""43d56880-093c-4428-b23d-f2f3e48cb2c2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cb550148-a8eb-4ab2-88ba-947b839a2f26"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Horizontal Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""830a44d7-d00a-4f4e-9883-bf23e8d30943"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac1c41f9-c9a9-4dcf-8a89-f511f43b3bd5"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6824ad36-fb71-451a-8b87-82ae3e25cddd"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa3ffa14-a36e-4d19-ad23-3822e00da06e"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca4352d8-c382-445b-9007-8a6ee1fd2838"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""FlipGravity"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // playerMovment
        m_playerMovment = asset.FindActionMap("playerMovment", throwIfNotFound: true);
        m_playerMovment_HorizontalMovement = m_playerMovment.FindAction("Horizontal Movement", throwIfNotFound: true);
        m_playerMovment_Jump = m_playerMovment.FindAction("Jump", throwIfNotFound: true);
        m_playerMovment_Dash = m_playerMovment.FindAction("Dash", throwIfNotFound: true);
        m_playerMovment_Slide = m_playerMovment.FindAction("Slide", throwIfNotFound: true);
        m_playerMovment_Slam = m_playerMovment.FindAction("Slam", throwIfNotFound: true);
        m_playerMovment_FlipGravity = m_playerMovment.FindAction("FlipGravity", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // playerMovment
    private readonly InputActionMap m_playerMovment;
    private List<IPlayerMovmentActions> m_PlayerMovmentActionsCallbackInterfaces = new List<IPlayerMovmentActions>();
    private readonly InputAction m_playerMovment_HorizontalMovement;
    private readonly InputAction m_playerMovment_Jump;
    private readonly InputAction m_playerMovment_Dash;
    private readonly InputAction m_playerMovment_Slide;
    private readonly InputAction m_playerMovment_Slam;
    private readonly InputAction m_playerMovment_FlipGravity;
    public struct PlayerMovmentActions
    {
        private @MovementInputActions m_Wrapper;
        public PlayerMovmentActions(@MovementInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMovement => m_Wrapper.m_playerMovment_HorizontalMovement;
        public InputAction @Jump => m_Wrapper.m_playerMovment_Jump;
        public InputAction @Dash => m_Wrapper.m_playerMovment_Dash;
        public InputAction @Slide => m_Wrapper.m_playerMovment_Slide;
        public InputAction @Slam => m_Wrapper.m_playerMovment_Slam;
        public InputAction @FlipGravity => m_Wrapper.m_playerMovment_FlipGravity;
        public InputActionMap Get() { return m_Wrapper.m_playerMovment; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovmentActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMovmentActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMovmentActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMovmentActionsCallbackInterfaces.Add(instance);
            @HorizontalMovement.started += instance.OnHorizontalMovement;
            @HorizontalMovement.performed += instance.OnHorizontalMovement;
            @HorizontalMovement.canceled += instance.OnHorizontalMovement;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Slide.started += instance.OnSlide;
            @Slide.performed += instance.OnSlide;
            @Slide.canceled += instance.OnSlide;
            @Slam.started += instance.OnSlam;
            @Slam.performed += instance.OnSlam;
            @Slam.canceled += instance.OnSlam;
            @FlipGravity.started += instance.OnFlipGravity;
            @FlipGravity.performed += instance.OnFlipGravity;
            @FlipGravity.canceled += instance.OnFlipGravity;
        }

        private void UnregisterCallbacks(IPlayerMovmentActions instance)
        {
            @HorizontalMovement.started -= instance.OnHorizontalMovement;
            @HorizontalMovement.performed -= instance.OnHorizontalMovement;
            @HorizontalMovement.canceled -= instance.OnHorizontalMovement;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Slide.started -= instance.OnSlide;
            @Slide.performed -= instance.OnSlide;
            @Slide.canceled -= instance.OnSlide;
            @Slam.started -= instance.OnSlam;
            @Slam.performed -= instance.OnSlam;
            @Slam.canceled -= instance.OnSlam;
            @FlipGravity.started -= instance.OnFlipGravity;
            @FlipGravity.performed -= instance.OnFlipGravity;
            @FlipGravity.canceled -= instance.OnFlipGravity;
        }

        public void RemoveCallbacks(IPlayerMovmentActions instance)
        {
            if (m_Wrapper.m_PlayerMovmentActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMovmentActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMovmentActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMovmentActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMovmentActions @playerMovment => new PlayerMovmentActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamePadSchemeIndex = -1;
    public InputControlScheme GamePadScheme
    {
        get
        {
            if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
            return asset.controlSchemes[m_GamePadSchemeIndex];
        }
    }
    public interface IPlayerMovmentActions
    {
        void OnHorizontalMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
        void OnSlam(InputAction.CallbackContext context);
        void OnFlipGravity(InputAction.CallbackContext context);
    }
}
