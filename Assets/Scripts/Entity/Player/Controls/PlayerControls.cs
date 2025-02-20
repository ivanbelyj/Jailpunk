using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private LayerMask mouseSelectionLayerMask;

    [SerializeField]
    private CloseCombat closeCombat;

    [SerializeField]
    private GridPhysicalMovement movement;

    [SerializeField]
    private Interactor interactor;

    [SerializeField]
    private PlayerInput playerInput;
    
    [SerializeField]
    private MoveControls moveControls;

    private ConsoleManager consoleManager;

    private Camera playerCamera;

    private CommunicationUIManager communicationUIManager;

    /// <summary>
    /// If true, some controls will be blocked due to input performing
    /// (in particular, keyboard input in a text input field)
    /// </summary>
    public bool IsTextInputMode { get; set; }

    public bool IsMouseInputDisabled { get; set; }

    // private GridManager gridManager;
    // private GridManager GridManager {
    //     get {
    //         if (gridManager == null)
    //             gridManager = GameObject.Find("GridManager")
    //                 .GetComponent<GridManager>();
    //         return gridManager;
    //     }
    // }

    public void OnToggleCommunicationUI(InputAction.CallbackContext context)
    {
        if (IsTextInputMode) return;

        if (context.phase == InputActionPhase.Performed) {
            communicationUIManager.ToggleUI();
        }
    }

    public void OnSelectNumber(InputAction.CallbackContext context) {
        if (IsTextInputMode) return;

        if (context.phase == InputActionPhase.Performed) {
            int.TryParse(context.control.name, out int num);
            communicationUIManager.CommunicationPanel.MakeChoiceByNumber(num);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (IsTextInputMode
            || IsMouseInputDisabled && context.control.device is Mouse) 
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed) {
            closeCombat.Attack();
        }
    }

    public void OnToggleConsole(InputAction.CallbackContext context) {
        if (IsTextInputMode) return;

        if (context.phase == InputActionPhase.Performed) {
            consoleManager.ToggleConsole();
        }
    }

    private void Awake() {
        playerCamera = Camera.main;
        communicationUIManager = FindObjectOfType<CommunicationUIManager>();
        consoleManager = FindObjectOfType<ConsoleManager>();
    }

    private void Update() {
        if (IsTextInputMode) return;

        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        moveControls.Move(moveInput);

        if (!IsMouseInputDisabled) {
            HandleMouseInput();
        }

        // int choiceNumber = playerInput.actions["Choice"].ReadValue<int>();
        // if (choiceNumber > 0) {
        //     Debug.Log("choice number: " + choiceNumber);
        // }
    }

    private void HandleMouseInput() {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
           Vector3 mousePosition = mouse.position.ReadValue();
           TryToInteract(mousePosition);
        }
    }

    private void TryToInteract(Vector3 mousePosition) {
        Vector2 pointInWorld = playerCamera.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pointInWorld, Vector2.zero,
            Mathf.Infinity, mouseSelectionLayerMask);
        
        GameObject interactable = hit.collider?.transform.parent.gameObject;
        if (interactable != null
            && interactor.IsAvailableToInteract(interactable)) {
            interactable.GetComponent<IInteractable>().ActivateIfAllowed();
        }
    }

    // private void Move(Vector2 moveInput) {
    //     movement.MovementInputValues = GridUtils.RotateCartesian(
    //         new Vector3(moveInput.x, moveInput.y, 0));
    // }
}
