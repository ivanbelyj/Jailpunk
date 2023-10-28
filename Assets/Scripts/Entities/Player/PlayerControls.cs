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
    private CharacterControls characterControls;
    private Interactor interactor;
    private AttachCameraOnStartLocalPlayer attachCamera;
    private Camera playerCamera;

    // private GridManager gridManager;
    // private GridManager GridManager {
    //     get {
    //         if (gridManager == null)
    //             gridManager = GameObject.Find("GridManager")
    //                 .GetComponent<GridManager>();
    //         return gridManager;
    //     }
    // }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        // movement = GetComponent<GridPhysicalMovement>();
        characterControls = GetComponent<CharacterControls>();
        interactor = GetComponent<Interactor>();
        attachCamera = GetComponent<AttachCameraOnStartLocalPlayer>();
        playerCamera = Camera.main;
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
