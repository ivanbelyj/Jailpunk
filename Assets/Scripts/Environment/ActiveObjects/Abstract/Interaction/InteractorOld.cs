// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using Mirror;

// public class InteractorOld : MonoBehaviour
// {
//     [SerializeField]
//     private float interactionRadius;

//     /// <summary>
//     /// The last objects that could be interacted with
//     /// </summary>
//     private HashSet<IInteractable> lastVisibleToInteract = null;

//     // Works in OnDrawGizmos
//     private GridManager gridManager;
//     private GridManager GridManager {
//         get {
//             if (gridManager == null)
//                 gridManager = GameObject.Find("GridManager")
//                     .GetComponent<GridManager>();
//             return gridManager;
//         }
//     }

//     /// <summary>
//     /// Interact with available interactable object
//     /// </summary>
//     public void Interact(IInteractable interactable) {
//         if (lastVisibleToInteract.Contains(interactable)
//             && interactable.State == ActivationState.ReadyToActivate
//             && interactable.IsObvious) {
//             interactable.Activate();
//         }
//     }

//     private void Update() {
//         UpdateImpl(false);
//     }

//     private void UpdateImpl(bool drawGizmos = false) {
//         Vector2 interactorPos = transform.position;
//         Collider2D[] overlappedByCircle = Physics2D.OverlapCircleAll(
//             interactorPos,
//             interactionRadius);
//         var interactables = overlappedByCircle
//             .Where(collider => {
//                 bool isInteractable = collider.GetComponent<IInteractable>() != null;
//                 if (!isInteractable)
//                     return false;

//                 // Check is interactable in radius
//                 Vector2 interactablePos = collider.transform.position;
//                 bool isDistanceLess = IsDistanceInGridLessThanInteractionRadius(
//                     interactorPos, interactablePos);
//                 // bool isDistanceLess = Vector2.Distance(interactorPos,
//                 //     interactablePos) < interactionRadius;
                
//                 if (drawGizmos) {
//                     Color prevCol = Gizmos.color;
//                     Gizmos.color = isDistanceLess ? Color.green : Color.red;
//                     Gizmos.DrawLine(interactablePos, interactorPos);
//                     Gizmos.color = prevCol;
//                 }
                
//                 return isDistanceLess;
//             })
//             .Select(collider => collider.GetComponent<IInteractable>());
        
//         UpdateVisualEffects(interactables);
//     }

//     private bool IsDistanceInGridLessThanInteractionRadius(Vector2 v1, Vector2 v2) {
//         Vector2 v1InGrid = GridManager.Vector2ToCartesian(v1);
//         Vector2 v2InGrid = GridManager.Vector2ToCartesian(v2);
//         return Vector2.Distance(v1InGrid, v2InGrid) < interactionRadius;
//     }

//     private void OnDrawGizmos() {
//         UpdateImpl(true);
//     }

//     private void UpdateVisualEffects(IEnumerable<IInteractable> currentInteractables) {
//         HashSet<IInteractable> newVisibleToInteract = new HashSet<IInteractable>();
//         foreach (IInteractable interactable in currentInteractables) {
//             interactable.UpdateVisualEffectsForCurrentState(true);
//             newVisibleToInteract.Add(interactable);
//         }

//         // Clear previous visual effects
//         if (lastVisibleToInteract != null) {
//             foreach (IInteractable interactable in lastVisibleToInteract) {
//                 if (!newVisibleToInteract.Contains(interactable)) {
//                     interactable.UpdateVisualEffectsForCurrentState(false);
//                 }
//             }
//         }

//         lastVisibleToInteract = newVisibleToInteract;
//     }
// }
