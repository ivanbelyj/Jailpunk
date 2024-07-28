using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GridPhysicalMovement))]
[RequireComponent(typeof(Interactor))]
public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private LayerMask mouseSelectionLayerMask;

    private PlayerInput playerInput;
    // private GridPhysicalMovement movement;
    private MoveControls characterControls;
    private Interactor interactor;
    private AttachCameraOnStartLocalPlayer attachCamera;
    private Camera playerCamera;

    private CommunicationUIManager communicationUIManager;


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
        if (context.phase == InputActionPhase.Performed) {
            communicationUIManager.ToggleUI();
        }
    }

    public void OnSelectNumber(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            int.TryParse(context.control.name, out int num);
            communicationUIManager.CommunicationPanel.MakeChoiceByNumber(num);
        }
        
    }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        // movement = GetComponent<GridPhysicalMovement>();
        characterControls = GetComponent<MoveControls>();
        interactor = GetComponent<Interactor>();
        attachCamera = GetComponent<AttachCameraOnStartLocalPlayer>();
        playerCamera = Camera.main;
        communicationUIManager = FindObjectOfType<CommunicationUIManager>();
    }

    private void Update() {
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        characterControls.Move(moveInput);

        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
           Vector3 mousePosition = mouse.position.ReadValue();
           TryToInteract(mousePosition);
        }

        // int choiceNumber = playerInput.actions["Choice"].ReadValue<int>();
        // if (choiceNumber > 0) {
        //     Debug.Log("choice number: " + choiceNumber);
        // }
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
